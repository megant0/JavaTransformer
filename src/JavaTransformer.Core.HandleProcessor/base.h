#ifndef _BASE_H_
#define _BASE_H_

#pragma region Include
	#ifndef _STRING_
		#include <string>
	#endif 
	#ifndef _VECTOR_
		#include <vector>
	#endif 
#pragma endregion

#define TARGET_VERSION 0.1
#define TARGET_NAME "Core"

#define _SPACE_W _PLUS_W _NONE_WORD_W _PLUS_W

typedef std::string		String;
typedef const char*		literal;
typedef uint16_t		u16;
typedef uint8_t			u8;

	/*CONSTRUCT*/

#define WCONVERT_STRING(body) std::string(body)

#define _THIS *this
#define FATAL_ERROR [[noreturn]]
#define CHECK_RETURN [[nodiscard]]
#define DEPRECATED [[deprecated]]
#define UNUSED [[maybe_unused]]
#define WCHECK_EX try {
#define DEFAULT_METHOD __thiscall
#define WEND_EX }catch(std::exception __error__) {}

#define USE_REGISTRY __fastcall

#define _PLUS_W +
#define _NONE_WORD_W " "

using std::vector;
#define arhive vector

#endif 
