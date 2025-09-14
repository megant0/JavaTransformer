#include <iostream>
#include <filesystem>
#include <vector>

#include "lib.h"

arhive<LIBData> datas;
arhive<StackTrace> stack;
namespace fs = std::filesystem;
String libFolder = "libraries";

libraries::libraries(String _libFolder)
{
    libFolder = _libFolder;
}

bool libraries::registry(const literal& name)
{
    LIBData data;
    data.name = name;

    String path = (WCONVERT_STRING(libFolder) + "\\" + name);

    if (fs::exists(path)) 
        if (!(fs::is_directory(path))) {
            stack.push_back(StackTrace{"Registry: " + path, false});
            return false;
        }
    
    data.path = fs::absolute(path).string();
    
    datas.push_back(data);
    stack.push_back(StackTrace{ "Registry: " + path, true });
    return true;
}

bool libraries::registryFastExe(const literal& name, const literal& fsExe)
{
    String buf 
        = WCONVERT_STRING(name);
    
    for (auto& _ : datas) {
        if (_.name == buf)
        {
            try {
                String fx = _.path + "\\" + fsExe;
                _.fastExe = fs::canonical(fx).string();
                stack.push_back(StackTrace{ "Registry Fast Exe: " + fx, true });
                return true;
            }
            catch (...) 
            {
                stack.push_back(StackTrace{ "Registry Fast Exe: " + WCONVERT_STRING(fsExe), false });
                return false; 
            }
        }
    }
    
    stack.push_back(StackTrace{ "Registry Fast Exe: " + WCONVERT_STRING(fsExe), false });
    return false;
}

arhive<literal>& libraries::getFolders()
{
    arhive<literal> buf;

    // ignore error
    for (const auto& _ : fs::directory_iterator(libFolder)) {
        if (_.is_directory())
            buf.push_back(_
                .path()
                .string()
                .c_str());
    }

    return buf;
}

arhive<StackTrace>& libraries::getStackTrace() 
{
    return stack;
}

LIBData libraries::getData(const String& name) {
    for(const auto &_ : datas) 
    {
        if (_.name == name)
            return _;
    }

    return NULL_LIB;
}

String libraries::getFile(const String& name, const String& file)  
{
    for (const auto& _ : datas)
    {
        if (_.name == name)
            return WCONVERT_STRING(libFolder) +"\\" + _.name + "\\" + file;
    }

    return "";
}

String libraries::path(const String& ex)
{
    return WCONVERT_STRING(libFolder) + "\\" + ex;
}
