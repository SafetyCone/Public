#include "stdafx.h"

#include "TestClass.h"


using namespace std;


CPPDLL_API void WriteToConsoleGlobal()
{
	cout << "Hello world!" << endl;
}

//CPPDLL_API void WriteStringToConsoleGlobal(string value)
//{
//	cout << value << endl;
//}

CPPDLL_API void WriteCharArrayToConsoleGlobal(char* value)
{
	cout << value << endl;
}


void TestClass::WriteToConsole()
{
	cout << "Hello world!" << endl;
}

void TestClass::WriteToConsole(string value)
{
	cout << value << endl;
}