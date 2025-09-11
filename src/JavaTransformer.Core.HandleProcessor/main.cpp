#include "entrypoint.h"

int main(int argc, char** argv) 
{
	return EntryPoint::launch(SST
	{ 
		argc, 
		argv 
	}, argv);
}