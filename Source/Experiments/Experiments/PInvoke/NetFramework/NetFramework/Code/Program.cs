using System;
using System.Runtime.InteropServices;


namespace NetFramework
{
    class Program
    {
        [DllImport("CppDLL.dll")]
        private static extern void WriteToConsoleGlobal();

        // Does not work to directly marshall strings.
        //[DllImport("CppDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void WriteStringToConsoleGlobal([MarshalAs(UnmanagedType.LPStr)] string value);

        //[DllImport("CppDLL.dll")] // Does not work. Requires the 
        [DllImport("CppDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void WriteCharArrayToConsoleGlobal([MarshalAs(UnmanagedType.LPStr)] string value);

        private static extern void WriteCharArrayToConsoleGlobal(string value);


        static void Main(string[] args)
        {
            Program.WriteToConsoleGlobal();
            //Program.WriteStringToConsoleGlobal("Hello again!");
            Program.WriteCharArrayToConsoleGlobal("Hello once more!");
        }
    }
}
