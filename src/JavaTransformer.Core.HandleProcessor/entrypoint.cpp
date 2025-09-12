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
#include <filesystem>

namespace fs = std::filesystem;

String getShortPath(const String& longPath) {
    char shortPath[MAX_PATH];
    if (GetShortPathNameA(longPath.c_str(), shortPath, MAX_PATH) > 0) {
        return String(shortPath);
    }
    return longPath; // fallback
}

namespace Mode {
    enum {
        DLL, EXE, DRIVER, JVM
    };
}

int EntryPoint::launch(SST args, char** argv)
{
    setlocale(LC_ALL, "Ru");
    int argc = args.size;

    if (argc < 2) {
        std::cerr << "Недостаточное кол-во аргументов." << std::endl;
        std::cerr << "Использование: " << argv[0] << " [dll|exe]" << std::endl;
        return 101;
    }

    literal flag = argv[1];
    
    int mode;

    if (strcmp(flag, "dll") == 0 || strcmp(flag, "DLL") == 0) {
        mode = Mode::DLL;
    }
    else if (strcmp(flag, "exe") == 0 || strcmp(flag, "EXE") == 0) {
        mode = Mode::EXE;
    }
    else if (strcmp(flag, "krn") == 0 || strcmp(flag, "KRN") == 0) {
        mode = Mode::DRIVER;
    }
    else if (strcmp(flag, "jvm") == 0 || strcmp(flag, "JVM") == 0) {
        mode = Mode::JVM;
    }
    else {
        std::cerr << "Error args mode use: [dll/DLL | exe/EXE]" << std::endl;
        return -11;
    }

    std::string input, outpute,
        meta, classloader,
        compilerProgram, dllcompProgram, 
        execompProgram, javaProgram, jdkInput,
        loadMethod, loadClass;

    try {
        std::ifstream configFile("config/generate_code.txt");
        if (!configFile.is_open()) {
            std::cerr << "Не удалось открыть файл конфигурации: config/generate_code.txt" << std::endl;
            return 104;
        }

        std::string line;

        while (std::getline(configFile, line)) {
            std::istringstream iss(line);
            std::string key, value;

            if (std::getline(iss, key, '=') && std::getline(iss, value)) {
                if (key == "input") input = value;
                else if (key == "output") outpute = value;
                else if (key == "meta") meta = value;
                else if (key == "classloader") classloader = value;
                else if (key == "compiler") compilerProgram = value;
                else if (key == "dllcomp") dllcompProgram = value;
                else if (key == "execomp") execompProgram = value;
                else if (key == "javacomp") javaProgram = value;
                else if (key == "jdkInput") jdkInput = value;
                else if (key == "loadMethod") loadMethod = value;
                else if (key == "loadClass") loadClass = value;
            }
        }
        if (meta.empty() && !loadClass.empty() && !loadMethod.empty()) 
            meta = loadClass + "[" + loadMethod;

        configFile.close();

        if (input.empty() || outpute.empty()) {
            std::cerr << "В конфигурационном файле отсутствуют обязательные параметры (input, output)" << std::endl;
            return 105;
        }

        if (mode == Mode::DLL && meta.empty()) {
            std::cerr << "Для DLL режима требуется параметр meta в конфигурационном файле" << std::endl;
            return 106;
        }
        if (mode == Mode::JVM && jdkInput.empty()) {
            std::cerr << "Для JVM режима требуется параметр jdkInput в конфигурационном файле" << std::endl;
            return 107;
        }


#define PROGRAM_DEFAULT "default"

        if (dllcompProgram.empty()) dllcompProgram = PROGRAM_DEFAULT;
        if (execompProgram.empty()) execompProgram = PROGRAM_DEFAULT;
        if (javaProgram.empty()) javaProgram = PROGRAM_DEFAULT;
    }
    catch (const std::exception& e) {
        std::cerr << "Ошибка чтения конфигурационного файла: " << e.what() << std::endl;
        return 107;
    }

    libraries lib;

    auto libNames = { "hx", "dll", "ldr", "jvm" };
    for (const auto& name : libNames) {
        lib.registry(name);
    }

    lib.registryFastExe("hx", "JavaTransformer.Core.HexDumper.exe");
    lib.registryFastExe("dll", "header.h");
    lib.registryFastExe("ldr", "header.h");
    lib.registryFastExe("jvm", "header.h");

    for (auto entry : lib.getStackTrace()) {
        if (!entry.state) {
            CrashLog::Create(entry.action);
            return -15;
        }
    }

    auto dll = lib.getData("dll");
    auto hex = lib.getData("hx");
    auto ldr = lib.getData("ldr");
    auto jvm = lib.getData("jvm");

    String outpute_dll = WCONVERT_STRING(outpute) + ".dll";
    String outpute_exe = WCONVERT_STRING(outpute) + ".exe";
    String pathToLastHex = "includes/last.txt";

    if (mode == Mode::DLL)
    {
        std::cout << "Compiling DLL\n";

        String pathToHeader;
       
        if (dllcompProgram == PROGRAM_DEFAULT)
            pathToHeader = dll.fastExe;
        else pathToHeader = dllcompProgram + std::string("\\header.h");

        winapi::bat(winapi::fix(hex.fastExe), WCONVERT_STRING(input) + WCONVERT_STRING(" includes/last.txt"));

        String start_1 = "#pragma once\n";

        arhive<String> metas = Split(WCONVERT_STRING(meta), "[");
        if (metas.size() < 2) {
            std::cerr << "Неверный формат meta параметра. Ожидается: class[method" << std::endl;
            return 108;
        }

        String cls = metas[0];
        String mht = metas[1];

        start_1 += "\n#define __CLASS_LOADER " + classloader;
        start_1 += "\n#define __CLASS__ \"" + cls + "\"";
        start_1 += "\n#define __METHOD__ \"" + mht + "\"";

        auto start_2 = readFile(pathToLastHex);
        String generated = start_1 + "\n" + start_2;

        clearFile(pathToHeader);
        appendToFile(pathToHeader, generated);

        String libFolder;


        if (dllcompProgram == PROGRAM_DEFAULT)
            libFolder = lib.path("dll");
        else libFolder = dllcompProgram;


        std::cout << libFolder << " | " << pathToHeader << "\n";
        compiler compDLL(compilerProgram);
        compDLL.enabledStatic()
            .linkDirectory(libFolder)
            .preArgs("-std=c++20")
            .setOutpute(outpute_dll)
            .enabledStaticLibgcc()
            .enabledStaticLibstdc()
            .preArgs("-w")
            .preArgs("-static -fno-exceptions -fno-rtti")
            .postArgs("-Lincludes -ljvm")
            .preArgs("-fpermissive")
            .postArgs("-O3")
            .postArgs("-march=native")
            .postArgs("-Wall")
            .postArgs("-Wextra")
            .postArgs("-s")
            .postArgs("-DNDEBUG")
            .postArgs("-pipe")
            .setBuildDLL(1);

        return compDLL.run();
    }
    else if (mode == Mode::EXE)
    {
        std::cout << "Compiling EXE\n";

        String pathToHeaderExe;

        if (execompProgram == PROGRAM_DEFAULT)
            pathToHeaderExe = ldr.fastExe;
        else pathToHeaderExe = execompProgram + std::string("\\header.h");

        winapi::bat(winapi::fix(hex.fastExe), outpute_dll + WCONVERT_STRING(" includes/last.txt"));

        String start_11 = "#pragma once\n/*GENERATED HEADER FILE*/\t";
        auto start_22 = readFile(pathToLastHex);

        String generated2 = start_11 + "\n" + start_22;
        clearFile(pathToHeaderExe);
        appendToFile(pathToHeaderExe, generated2);

        String libFolder;
        if (execompProgram == PROGRAM_DEFAULT)
            libFolder = lib.path("ldr");
        else libFolder = execompProgram;

        compiler compEXE(compilerProgram);
        compEXE.enabledStatic()
            .linkDirectory(libFolder)
            .preArgs("-std=c++20")
            .setOutpute(outpute_exe)
            .enabledStaticLibgcc()
            .enabledStaticLibstdc()
            .preArgs("-w")
            .preArgs("-fpermissive");

        return compEXE.run();
    }
    else if (mode == Mode::JVM) 
    {
        std::cout << "Patch JVM\n";

        String pathToHeaderExe;
        
        if (javaProgram == PROGRAM_DEFAULT)
            pathToHeaderExe = jvm.fastExe;
        else pathToHeaderExe = javaProgram + std::string("\\header.h");

        winapi::bat(winapi::fix(hex.fastExe), outpute_dll + WCONVERT_STRING(" includes/last.txt"));

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
        auto file_size = std::filesystem::file_size(outputFile);

        if (file_size > 500 * 1024) {
            CrashLog::Create("Отказано в доступе.\nПодозрение на рекурсию.");
            return 0x0005;
        }
        DeleteFileA(javaw.c_str());
        rename(outputFile.c_str(), javaw.c_str());

        compiler compEXE(compilerProgram);
        compEXE.enabledStatic()
            .linkDirectory(libFolder)
            .preArgs("-std=c++20")
            .setOutpute(outputFile)
            .enabledStaticLibgcc()
            .enabledStaticLibstdc()
            .preArgs("-w")
            .preArgs("-fpermissive");

        return compEXE.run();
    }
    else if (mode == Mode::DRIVER) 
    {

    }
}