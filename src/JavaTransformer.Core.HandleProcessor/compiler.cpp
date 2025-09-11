#include <filesystem>
#include <iostream>

#include "compiler.h"
#include "winapi.h"

arhive<String> _postArgs;
arhive<String> _preArgs;
arhive<String> _linkFiles;
arhive<String> _accessFiles;

bool constainsLinkFile(const String& _);

String input;
String outpute;

compiler::compiler(const String& _)
{
	checkAPK(_);
}

compiler compiler::postArgs(const String& _)
{
	_postArgs.push_back(_);
	return _THIS;
}

compiler compiler::preArgs(const String& _)
{
	_preArgs.push_back(_);
	return _THIS;
}

compiler compiler::linkFile(const String& _)
{
	if (!constainsLinkFile(_)) {
		_linkFiles.push_back(_);
	}
	return _THIS;
}

compiler compiler::linkDirectory(const String& _)
{
	for (const auto& iterator : std::filesystem::directory_iterator(_))
	{
		if (iterator.is_regular_file())
		{
			String extension = iterator.path().extension().string();
			if (extension == ".cpp" || extension == ".c" || extension == ".cc" ||
				extension == ".cxx" || extension == ".c++") {
				if (!constainsLinkFile(iterator.path().string())) {
					_linkFiles.push_back(iterator.path().string());
				}
			}
		}
	}
	return _THIS;
}

bool constainsLinkFile(const String& _) {
	if (input == (_))				 // swag
	{
		return true;
	}

	for (const auto& _2 : _accessFiles) {
		if (_2.find(_))				// karnaval
			return true;
	}
	for (const auto& _2 : _linkFiles) {
		if (_2 == _)
			return true;
	}
	
	return false;
}

compiler compiler::setInput(const String& _)
{
	input = _;
	_accessFiles.push_back(_);

	return _THIS;
}

compiler compiler::setOutpute(const String& _)
{
	outpute = _;
	return _THIS;
}

compiler compiler::setSTDVersion(std_version _)
{
	switch (_)
	{
	case STD_98:
		preArgs("-std=c++98");
		break;
	case STD_11:
		preArgs("-std=c++11");
		break;
	case STD_14:
		preArgs("-std=c++14");
		break;
	case STD_17:
		preArgs("-std=c++17");
		break;
	case STD_20:
		preArgs("-std=c++20");
		break;
	case STD_23:
		preArgs("-std=c++23");
		break;
	default:
		break;
	}
	return _THIS;
}

compiler compiler::setBuildDLL(const flag_i& _)
{
	if(_ == true)
	preArgs("-shared");

	return _THIS;
}

compiler compiler::setOptimizeComp(const flag_optimize& fl)
{
	switch (fl)
	{
	case DEFAULT:
		postArgs("-O0");
		break;
	case LEVEL_1:
		postArgs("-O1");
		break;
	case LEVE_2:
		postArgs("-O2");
		break;
	case LEVEL_3:
		postArgs("-O3");
		break;
	case LEVEL_S:
		postArgs("-OS");
		break;
	case FULL_FAST:
		postArgs("-Ofast");
		break;
	default:
		break;
	}

	return _THIS;
}

compiler compiler::enabledStatic()
{
	postArgs("-static");
	return _THIS;
}

compiler compiler::enabledStaticLibgcc()
{
	postArgs("-static-libgcc");
	return _THIS;
}

compiler compiler::enabledStaticLibstdc()
{
	postArgs("-static-libstdc++");
	return _THIS;
}

compiler compiler::enabledNostdlib()
{

	postArgs("-nostdlib");
	return _THIS;
}

compiler compiler::enabledNostartfiles()
{
	postArgs("-nostartfiles");
	return _THIS;
}

compiler compiler::enabledNodefaultlibs()
{
	postArgs("-nodefaultlibs");
	return _THIS;
}

compiler compiler::registryLibrary(const String& _)
{
	preArgs("-l " + _);
	return _THIS;
}

String compiler::getPath()
{
	return apk;
}

String compiler::getCommandLine()
{
	String pre;
	String post;
	String links;

	for(const auto &_ : _preArgs) 
		pre += _SPACE_W _;

	for (const auto& _ : _postArgs)
		post += _SPACE_W _;

	for (const auto& _ : _linkFiles)
		links += _SPACE_W _;
	
	String p = pre
		_SPACE_W input
		_SPACE_W links
		_SPACE_W "-o"
		_SPACE_W outpute
		_SPACE_W post;

	return p;
}

flag_i compiler::run()
{
	return winapi::bat(apk, getCommandLine());
}

bool compiler::checkAPK(String _)
{
	String buf = winapi::fix(_);

	apk = buf;
	return true;
}
