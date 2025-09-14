#ifndef _LIB_H_W_
#define _LIB_H_W_

// base.h
	 #include "base.h"


#define NONE_FAST_EXE "NONE";

typedef struct 
{
	String name;
	String path;
	String fastExe;
}LIBData;

typedef struct {
	String action;
	bool state;
}StackTrace;

static const LIBData libnull{ "", "", "" };

#define NULL_LIB libnull;

class libraries 
{
public:
	libraries(String _libFolder);

	bool					registry(const literal& name);
	bool					registryFastExe(const literal& name, const literal& fsExe);

	arhive<literal>&		getFolders();
	arhive<StackTrace>&		getStackTrace();
	LIBData 				getData(const String& name);

	String					getFile(const String& name, const String& file);
	String					path(const String &ex);
};

#endif 

