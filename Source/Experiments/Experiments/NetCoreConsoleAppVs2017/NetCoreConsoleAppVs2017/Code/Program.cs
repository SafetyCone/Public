using System;

using ClassLibrary1Vs2017;


namespace NetCoreConsoleAppVs2017
{
    class Program
    {
        static void Main(string[] args)
        {
            UsefulClass useful = new UsefulClass();

            Console.WriteLine(useful.Value);

            useful.Value = @"Something else.";

            Console.WriteLine(useful.Value);
        }
    }
}