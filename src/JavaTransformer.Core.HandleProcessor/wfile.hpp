#pragma once
#include <string>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <sstream>

    #include "base.h"

void appendToFile(const std::string& filename, const std::string& text) {
    std::ifstream inFile(filename);
    std::string content((std::istreambuf_iterator<char>(inFile)), std::istreambuf_iterator<char>());
    inFile.close();
    content = text + content;
    std::ofstream outFile(filename);
    outFile << content;
    outFile.close();
}

std::string readFile(const std::string& filePath, bool binaryMode = false) {
    std::ifstream file;
    if (binaryMode) 
      file.open(filePath, std::ios::binary);
    else 
      file.open(filePath);

    std::string content(
        (std::istreambuf_iterator<char>(file)),
        std::istreambuf_iterator<char>()
    );

    file.close(); // free
    return content;
}

bool clearFile(const std::string& filePath) {

    std::ofstream file(filePath, std::ios::trunc);
    return file.good(); 

}

arhive<String> Split(String _, String p) {
    arhive<String> tokens;
    std::stringstream ss(_);
    std::string token;

    char delimiter = p.empty() ? ' ' : p[0];

    while (std::getline(ss, token, delimiter)) {
        if (!token.empty()) {
            tokens.push_back(String(token));
        }
    }

    return tokens;
}