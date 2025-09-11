#include "winapi.h"
#include <iostream>

int winapi::bat(String path, String args)
{
    return sys((path _SPACE_W args).c_str());
}

int winapi::sys(String cmd)
{
    return std::system(cmd.c_str());
}

str winapi::fix(str _)
{
    return "\"" + _ + "\"";
}

int winapi::pss()
{
    return system("pause");
}
