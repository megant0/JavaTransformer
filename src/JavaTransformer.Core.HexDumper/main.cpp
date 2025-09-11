#include <iostream>
#include <fstream>
#include <iomanip>
#include <vector>
#include <filesystem>


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
    if (argc != 3) {
        std::cout << "use: \"hex.exe INPUT.FILE OUTPUT.C\"\n";
        return 3;
    }

    try {
        auto file_size = std::filesystem::file_size(argv[1]);

        std::ifstream input(argv[1], std::ios::binary);
        std::ofstream output(argv[2]);

        if (!input || !output) {
            return 2;
        }

        std::vector<unsigned char> buffer(file_size);
        input.read(reinterpret_cast<char*>(buffer.data()), file_size);

        output << "/* StartOffset(h): 00000000, EndOffset(h): "
            << std::uppercase << std::hex << std::setw(8) << std::setfill('0') << (file_size - 1)
            << ", Size(h): " << std::setw(8) << file_size << " */\n\n";

        v1(output, buffer);

        input.close();
        output.close();
        return 0;
    }
    catch (const std::exception& e) {
        return 1;
    }
}