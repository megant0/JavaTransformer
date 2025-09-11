#ifndef _ENTRY_POINT_H_
#define _ENTRY_POINT_H_

#define M_OK 0
#define M_ERROR -1

typedef struct {
	int size;
	char** args;
}SST;

class EntryPoint {
public:
	static int launch(SST args, char** arg);
};

#endif