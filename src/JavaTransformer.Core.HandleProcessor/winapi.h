#ifndef _WIN_API_H_
#define _WIN_API_H_

// base.h
#include "base.h"
#define str String

class winapi {
public:
	static int bat(String path, String args);
	static int sys(String cmd);
	static str fix(str _);
	static int pss();
};

#endif 