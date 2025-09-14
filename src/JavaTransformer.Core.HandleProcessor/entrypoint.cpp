#include <filesystem>
#include <thread>
#include <Windows.h>
#include <iostream>
#include <fstream>
#include <sstream>
#include "entrypoint.h"
#include "lib.h"
#include "winapi.h"
#include "compiler.h"
#include "wfile.hpp"
#include "Crashlog.h"
#include "parsing_config.h"
#include <unordered_map>
#include "common/parse_value.h"

namespace fs = std::filesystem;
static bool _debug = false;
#pragma region debug

    #define WRITE_DEB(str) \
if(_debug)\
std::cout << "[DEBUG] > " << str << '\n'; 


    #define WRITE_DEB2(str, str2) \
if(_debug) \
std::cout << "[DEBUG] > " << str << " " << str2 << '\n';  

#pragma endregion

#pragma region
namespace Mode {
    enum {
        DLL = 99,
        EXE = 98,
        DRIVER = 97, 
        JVM = 96, 
        NONE = -1
    };
}
#pragma endregion 

std::unordered_map<String, int> map_mode = {
    {"dll", Mode::DLL},
    {"DLL", Mode::DLL},
    {"exe", Mode::EXE},
    {"EXE", Mode::EXE},
    {"jvm", Mode::JVM},
    {"JVM", Mode::JVM},
    {"krn", Mode::DRIVER},
    {"KRN", Mode::DRIVER}
};

int parse_mode(int argc, char** argv)
{
    if (argc < 2) {
        return 101;
    }

    String flag = argv[1];

    if (map_mode.find(flag) != map_mode.end()) {
        return map_mode[flag];
    }

    return Mode::NONE;
}

int handle_compile_mode(const String& mode_name,
    const String& compiler_program,
    const String& lib_folder,
    const String& header_path,
    const String& output_file,
    bool is_dll = false,
    String custom_args = "")
{
    std::cout << "Compiling " << mode_name << "\n";

    compiler comp(compiler_program);
    comp.enabledStatic()
        .linkDirectory(lib_folder)
        .preArgs("-std=c++20")
        .setOutpute(output_file)
        .enabledStaticLibgcc()
        .enabledStaticLibstdc()
        .preArgs("-w")
        .preArgs("-fpermissive");

    comp.preArgs(custom_args);

    if (is_dll) {
        comp.preArgs("-static -fno-exceptions -fno-rtti")
            .postArgs("-Lincludes -ljvm")
            .postArgs("-O3")
            .postArgs("-march=native")
            .postArgs("-Wall")
            .postArgs("-Wextra")
            .postArgs("-s")
            .postArgs("-DNDEBUG")
            .postArgs("-pipe")
            .setBuildDLL(1);
    }

    return comp.run();
}

bool hasArgument(int argc, char** argv, const std::string& line)
{
    for (int i = 0; i < argc; i++)
        if (strcmp(argv[i], line.c_str()) == 0) 
            return true;

    return false;
}

std::string extractArgument(int argc, char** argv, const std::string& line) {
    for (int i = 0; i < argc; i++) {
        std::string left = argv[i];
        size_t pos = left.find(line);

        if (pos != std::string::npos) {
            if (pos == 0 && left.length() > line.length()) {
                return left.substr(line.length() + 1);
            }
        }
    }
    return "";
}

int EntryPoint::launch(SST args, char** argv)
{
    WRITE_DEB("launch...");
    _debug = hasArgument(args.size, argv, "--debug");

    setlocale(LC_ALL, "Ru");
    int argc = args.size;

    WRITE_DEB("parse mode...");
    int mode = parse_mode(argc, argv);

    WRITE_DEB("result: ");
    WRITE_DEB2("== ", mode);

    switch (mode)
    {
    case 101: {
        std::cerr << "Too few arguments." << std::endl;
        std::cerr << "Usage: " << argv[0] << " [dll|exe|jvm|krn]" << std::endl;
        return 101;
    }
    case Mode::NONE: {
        std::cerr << "Error args mode use: [dll|exe|jvm|krn]" << std::endl;
        return 104;
    }
    default:
        break;
    }

    WRITE_DEB("parsing config path...");

    std::string libraries_path = "";
    std::string includes_folder = "";
    std::string  crash_log_path = "";
    std::string     config_path = "";
    
    config_path = extractArgument(argc, argv, "config");
    libraries_path = extractArgument(argc, argv, "--libraries");
    crash_log_path = extractArgument(argc, argv, "--crashLog");
    includes_folder = extractArgument(argc, argv, "--includes");

    if (libraries_path.empty()) {
        libraries_path = "libraries";
        WRITE_DEB("not found libraries path!");
    }
    if (config_path.empty()) {
        config_path = "config/generate_code.txt";
        WRITE_DEB("not found config path!");
    }
    if (crash_log_path.empty()) {
        crash_log_path = "crashlog.txt";
        WRITE_DEB("not found crashlog path!");
    }
    if (includes_folder.empty()) {
        includes_folder = "includes";
        WRITE_DEB("not found includes path!");
    }

    WRITE_DEB2("config path    = ", config_path);
    WRITE_DEB2("libraries path = ", libraries_path);
    WRITE_DEB2("crash-log path = ", crash_log_path);
    WRITE_DEB2("includes path  = ", includes_folder);

    std::string input, outpute,
        meta, classloader,
        compilerProgram, dllcompProgram,
        execompProgram, javaProgram, jdkInput,
        loadMethod, loadClass;

    WRITE_DEB("parsing config params...");
    try
    {
        std::ifstream configFile(config_path);
        if (!configFile.is_open()) {
            std::cerr << "Error open configuration file: " << config_path << std::endl;
            return 104;
        }

        std::vector<String> config_keys = {
            "input", "output", "meta", "classloader", "compiler",
            "dllcomp", "execomp", "javacomp", "jdkInput", "loadMethod", "loadClass", "crashPath"
        };

        auto config_values = parsing_confing::parse_configs(std::move(configFile), config_keys, '=');

        input = config_values["input"];
        outpute = config_values["output"];
        meta = config_values["meta"];
        classloader = config_values["classloader"];
        compilerProgram = config_values["compiler"];
        dllcompProgram = config_values["dllcomp"];
        execompProgram = config_values["execomp"];
        javaProgram = config_values["javacomp"];
        jdkInput = config_values["jdkInput"];
        loadMethod = config_values["loadMethod"];
        loadClass = config_values["loadClass"];

        if (meta.empty() && !loadClass.empty() && !loadMethod.empty())
            meta = loadClass + "[" + loadMethod;

        if (input.empty() || outpute.empty()) {
            std::cerr << "Configuration file is missing required parameters (input, output)" << std::endl;
            return 105;
        }

        if (mode == Mode::DLL && meta.empty()) {
            std::cerr << "DLL mode requires the 'meta' parameter in the configuration file" << std::endl;
            return 106;
        }

        if (mode == Mode::JVM && jdkInput.empty()) {
            std::cerr << "JVM mode requires the 'jdkInput' parameter in the configuration file" << std::endl;
            return 107;
        }

#define PROGRAM_DEFAULT "default"

        if (dllcompProgram.empty()) dllcompProgram = PROGRAM_DEFAULT;
        if (execompProgram.empty()) execompProgram = PROGRAM_DEFAULT;
        if (javaProgram.empty()) javaProgram = PROGRAM_DEFAULT;
    }
    catch (const std::exception& e) {
        std::cerr << "Error reading configuration file: " << e.what() << std::endl;
        WRITE_DEB("error parsing config params:3");
        return 107;
    }
    WRITE_DEB("success parsing config params!");

    libraries lib(libraries_path);

    WRITE_DEB("create libraries");
    auto libNames = { "hx", "dll", "ldr", "jvm" };
    for (const auto& name : libNames) {
        lib.registry(name);
    }

    WRITE_DEB(" - registry libraries...");
    lib.registryFastExe("hx", "JavaTransformer.Core.HexDumper.exe");
    lib.registryFastExe("dll", "header.h");
    lib.registryFastExe("ldr", "header.h");
    lib.registryFastExe("jvm", "header.h");

    for (auto entry : lib.getStackTrace()) {

        WRITE_DEB2("  - check stack: ", entry.action);
        if (!entry.state) {
            CrashLog::Create(crash_log_path, entry.action);
            return -15;
        }
    }

    WRITE_DEB("  - get data libraries ");
    auto dll = lib.getData("dll");
    auto hex = lib.getData("hx");
    auto ldr = lib.getData("ldr");
    auto jvm = lib.getData("jvm");

    String outpute_dll = WCONVERT_STRING(outpute) + ".dll";
    String outpute_exe = WCONVERT_STRING(outpute) + ".exe";
    String pathToLastHex = includes_folder + "/last-hex.hex.h";

    WRITE_DEB("compiling...");

  WRITE_DEB("launch hex program...");

    auto start = std::chrono::high_resolution_clock::now();

    WRITE_DEB2("create cmd line launch hex.exe = ", WCONVERT_STRING(input) + " " + includes_folder + WCONVERT_STRING("\\last-hex.hex.h"))
        winapi::bat(winapi::fix(hex.fastExe), WCONVERT_STRING(input) + " " + includes_folder + WCONVERT_STRING("\\last-hex.hex.h"));

    auto end = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(end - start);

    WRITE_DEB2("success hex program! Time:", std::to_string(duration.count()) + "ms");
    if (mode == Mode::DLL)
    {
        WRITE_DEB("DLL");
        String pathToHeader;

        if (dllcompProgram == PROGRAM_DEFAULT)
            pathToHeader = dll.fastExe;
        else pathToHeader = dllcompProgram + std::string("\\header.h");

     


        String start_1 = "#pragma once\n";

        WRITE_DEB("analys args");

        arhive<String> metas = Split(WCONVERT_STRING(meta), "[");
        if (metas.size() < 2) {
            std::cerr << "Invalid meta parameter format. Expected: class[method" << std::endl;
            return 108;
        }

        String cls = metas[0];
        String mht = metas[1];

        start_1 += "\n#define __CLASS_LOADER " + classloader;
        start_1 += "\n#define __CLASS__ \"" + cls + "\"";
        start_1 += "\n#define __METHOD__ \"" + mht + "\"";

        WRITE_DEB("read hex");

        auto start_2 = readFile(pathToLastHex);
        String generated = start_1 + "\n" + start_2;

        WRITE_DEB("swap header");

        clearFile(pathToHeader);
        appendToFile(pathToHeader, generated);

        String libFolder;
        if (dllcompProgram == PROGRAM_DEFAULT)
            libFolder = lib.path("dll");
        else libFolder = dllcompProgram;

        std::cout << libFolder << " | " << pathToHeader << "\n";

        WRITE_DEB("handle compiling...");

        return handle_compile_mode("DLL", compilerProgram, libFolder, pathToHeader, outpute_dll, true, "-L/" + includes_folder + " -ljvm");
    }
    else if (mode == Mode::EXE)
    {
        String pathToHeaderExe;

        if (execompProgram == PROGRAM_DEFAULT)
            pathToHeaderExe = ldr.fastExe;
        else pathToHeaderExe = execompProgram + std::string("\\header.h");

        String start_11 = "#pragma once\n/*GENERATED HEADER FILE*/\t";
        auto start_22 = readFile(pathToLastHex);

        String generated2 = start_11 + "\n" + start_22;
        clearFile(pathToHeaderExe);
        appendToFile(pathToHeaderExe, generated2);

        String libFolder;
        if (execompProgram == PROGRAM_DEFAULT)
            libFolder = lib.path("ldr");
        else libFolder = execompProgram;

        return handle_compile_mode("EXE", compilerProgram, libFolder, pathToHeaderExe, outpute_exe);
    }
    else if (mode == Mode::JVM)
    {
        String pathToHeaderExe;

        if (javaProgram == PROGRAM_DEFAULT)
            pathToHeaderExe = jvm.fastExe;
        else pathToHeaderExe = javaProgram + std::string("\\header.h");

        String start_11 = "#pragma once\n/*GENERATED HEADER FILE*/\t";
        auto start_22 = readFile(pathToLastHex);

        String generated2 = start_11 + "\n" + start_22;
        clearFile(pathToHeaderExe);
        appendToFile(pathToHeaderExe, generated2);

        String libFolder;
        if (javaProgram == PROGRAM_DEFAULT)
            libFolder = lib.path("jvm");
        else libFolder = javaProgram;


        String outputFile = jdkInput + "\\bin\\java.exe";
        String javaw = jdkInput + "\\bin\\javaw.exe";

        try {
            auto file_size = std::filesystem::file_size(outputFile);
            if (file_size > 500 * 1024) {
                CrashLog::Create(crash_log_path, "Access denied.\nSuspected recursion.");
                return 0x0005;
            }
        }
        catch (const std::exception& e) {
            std::cerr << "Error accessing file: " << e.what() << std::endl;
            return 109;
        }

        DeleteFileA(javaw.c_str());
        if (rename(outputFile.c_str(), javaw.c_str()) != 0) {
            std::cerr << "Error renaming file" << std::endl;
            return 110;
        }

        return handle_compile_mode("JVM", compilerProgram, libFolder, pathToHeaderExe, outputFile);
    }
    else if (mode == Mode::DRIVER)
    {
        std::cout << "DRIVER mode not implemented yet" << std::endl;
        return 0;
    }

    return 0;
}