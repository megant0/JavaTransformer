#include <algorithm>
#include <cctype>
#include <string>
#include <locale>
#include "parse_value.h"

bool parse_values::parse_bool(const String& value) noexcept
{
    if (value == "true" || value == "1" || value == "yes" || value == "on")
        return true;
    if (value == "false" || value == "0" || value == "no" || value == "off")
        return false;

    std::string lowerValue;
    std::transform(value.begin(), value.end(), std::back_inserter(lowerValue),
        [](unsigned char c) { return std::tolower(c); });

    if (lowerValue == "true" || lowerValue == "yes" || lowerValue == "on")
        return true;

    return false;
}

int parse_values::parse_int32(const String& value) noexcept
{
    if (value.empty()) {
        return 0; 
    }

    size_t start = value.find_first_not_of(" \t\n\r");
    if (start == std::string::npos) {
        return 0; 
    }

    if (value == "true" || value == "TRUE") return 1;
    if (value == "false" || value == "FALSE") return 0;

    char* endptr = nullptr;
    errno = 0; 

    long result = std::strtol(value.c_str() + start, &endptr, 10);

    if (endptr == value.c_str() + start) {
        return 0;
    }

    std::string remaining(endptr);
    size_t non_space = remaining.find_first_not_of(" \t\n\r");
    if (non_space != std::string::npos) {
        return 0;
    }

    if (errno == ERANGE || result > INT_MAX || result < INT_MIN) {
        return 0; 
    }

    return static_cast<int>(result);
}
