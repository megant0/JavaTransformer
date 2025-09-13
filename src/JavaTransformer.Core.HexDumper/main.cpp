#include <iostream>
#include <fstream>
#include <iomanip>
#include <vector>
#include <filesystem>

static bool _debug_meta_header;
static bool _debug_define_header;

bool hasArgument(int* argc, char** argv, const char* text) {
    for (int i = 0; i < *argc; i++)
        if (strcmp(argv[i], text) == 0) return true;

    return false;
}

void v1(std::ofstream& output, const std::vector<unsigned char>& data) {
    const int bytesPerLine = 12;
    output << "#define __SIZE__ " << "0x" << std::uppercase << std::hex << data.size() << "\n\n\n";
    output << "static unsigned char rawData[" << "__SIZE__" << "] = {\n";

    for (size_t i = 0; i < data.size(); i += bytesPerLine) {
        output << "\t";
        for (size_t j = 0; j < bytesPerLine && (i + j) < data.size(); j++) {
            output << "0x" << std::uppercase << std::hex << std::setw(2) << std::setfill('0')
                << static_cast<int>(data[i + j]);
            if ((i + j + 1) < data.size() || j + 1 < bytesPerLine) {
                output << ", ";
            }
        }
        output << "\n";
    }
    output << "};\n";
}

int main(int argc, char* argv[]) {
    if (argc < 3) {
        std::cout << "use: \"hex.exe INPUT.FILE OUTPUT.C\"\n";
        return 3;
    }

    _debug_meta_header = hasArgument(&argc, argv, "--debug-header_comment");
    _debug_define_header = hasArgument(&argc, argv, "--debug-header_define");

    try {
        std::string file_path = argv[1];
        std::string output_path = argv[2];

        auto file_size = std::filesystem::file_size(file_path);

        std::ifstream input(file_path, std::ios::binary);
        std::ofstream output(output_path);

        if (!input || !output) {
            return 2;
        }

        std::vector<unsigned char> buffer(file_size);
        input.read(reinterpret_cast<char*>(buffer.data()), file_size);

        output << "/* StartOffset(h): 00000000, EndOffset(h): "
            << std::uppercase << std::hex << std::setw(8) << std::setfill('0') << (file_size - 1)
            << ", Size(h): " << std::setw(8) << file_size << " */\n\n";
        
        if (_debug_meta_header) {
            output << "/* --debug-meta: " << '\n';
            output << "input file=" << file_path << '\n';
            output << "output file=" << output_path << '\n';
            output << "*/" << '\n';
        }
        if (_debug_define_header) {
            output << "#define _HEX_DUMP_INPUT_FILE " << '"' << file_path << '"' << '\n';
            output << "#define _HEX_DUMP_OUTPUT_FILE " << '"' << output_path << '"' << '\n';
        }

        v1(output, buffer);


        input.close();
        output.close();
        return 0;
    }
    catch (const std::exception& e) {
        return 1;
    }
}