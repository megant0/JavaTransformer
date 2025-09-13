#pragma once

// base.h
	#include "../base.h"
namespace parse_values {
	bool parse_bool(const String& value) noexcept;
	int  parse_int32(const String& value) noexcept;
}