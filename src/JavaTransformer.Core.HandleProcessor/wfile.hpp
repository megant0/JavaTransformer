#pragma once
#include <string>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <sstream>

    #include "base.h"

String getShortPath(const String& longPath) {
    char shortPath[MAX_PATH];
    if (GetShortPathNameA(longPath.c_str(), shortPath, MAX_PATH) > 0) {
        return String(shortPath);
    }
    return longPath; // fallback
}

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