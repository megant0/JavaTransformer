#include "parsing_config.h"
#include <sstream>

String parsing_confing::parse_config(ifstream src, String searched, char delemitor)
{
    String line;
    while (std::getline(src, line)) {
        std::istringstream iss(line);
        std::string key, value;

        if (std::getline(iss, key, delemitor) && std::getline(iss, value))
        {
            if (key == searched) return value;
        }
    }
    return "";
}

using std::vector;
using std::unordered_map;

unordered_map<String, String> parsing_confing::parse_configs(ifstream src, vector<String> searched, char delemitor)
{
    String line;
    unordered_map<String, String> result = {};

    while (std::getline(src, line)) {
        std::istringstream iss(line);
        std::string key, value;

        if (std::getline(iss, key, delemitor) && std::getline(iss, value))
            for (const auto& item : searched)
                if (key == item)  result[key] = value;
    }
    return result;
}