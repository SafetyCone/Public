#pragma once

#include "CppDLL.h"

#include <string>

#ifdef __cplusplus
extern "C" {
#endif // _cplusplus

	CPPDLL_API void WriteToConsoleGlobal();
	//CPPDLL_API void WriteStringToConsoleGlobal(std::string value); // Does not work, need to use the C-linkage basic data type.
	CPPDLL_API void WriteCharArrayToConsoleGlobal(char* value); // Works, uses the C-linkage basic data type char*.

#ifdef __cplusplus
}
#endif


class CPPDLL_API TestClass
{
public:
	static void WriteToConsole();
	static void WriteToConsole(std::string value);
};