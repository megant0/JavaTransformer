#pragma once

#include <vector>
#include <unordered_map>
#include "base.h"
#include <fstream>

namespace parsing_confing {
	using std::ifstream;
	using std::vector;
	using std::unordered_map;

	String parse_config(ifstream src, String searched, char delemitor);
	unordered_map<String, String> parse_configs(ifstream src, vector<String> searched, char delemitor);
}