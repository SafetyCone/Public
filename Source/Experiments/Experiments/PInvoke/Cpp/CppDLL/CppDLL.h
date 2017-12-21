#pragma once

#ifdef CPPDLL_EXPORTS // Name was set by the Project file setup.
#define CPPDLL_API __declspec(dllexport) 
#else
#define CPPDLL_API __declspec(dllimport) 
#endif // DLIBDLL_EXPORTS