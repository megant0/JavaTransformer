#ifndef _COMPILER_H_
#define _COMPILER_H_

// base.h
	#include "base.h"
#include <string>

typedef int flag_i;

enum flag_optimize {DEFAULT, LEVEL_1, LEVE_2, LEVEL_3, LEVEL_S, FULL_FAST};
enum std_version {STD_98, STD_11, STD_14, STD_17, STD_20, STD_23};

class compiler {
public:
	compiler() = default;
	compiler(const String& _);

	compiler 		setOptimizeComp(const flag_optimize& fl);
	compiler								 enabledStatic();
	compiler						   enabledStaticLibgcc();
	compiler						  enabledStaticLibstdc();
	compiler							   enabledNostdlib();
	compiler						   enabledNostartfiles();
	compiler						  enabledNodefaultlibs();
	compiler			    registryLibrary(const String& _);
	compiler					   postArgs(const String& _);
	compiler						preArgs(const String& _);
	compiler					   linkFile(const String& _);
	compiler				  linkDirectory(const String& _);
	compiler				   	   setInput(const String& _);
	compiler					 setOutpute(const String& _);
	compiler					setSTDVersion(std_version _); 
	compiler					setBuildDLL(const flag_i& _);
	
	String getPath();

	String getCommandLine();

	flag_i run();
private:
	bool checkAPK(String _);
	
	String apk;
};

#endif

