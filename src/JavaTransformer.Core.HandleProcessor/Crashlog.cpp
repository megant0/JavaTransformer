#include "Crashlog.h"
#include <fstream>
#include <filesystem>
#include <iostream>

void CrashLog::Create(String path, String _)
{
    std::cout << "created crash log...\n";
    std::ofstream out;          
    out.open(path);      
    if (out.is_open())
    {
        out << "------ Crash Log -------\n\n\n" << std::endl;
        out << _ << "\n";
    }
    out.close();
}
