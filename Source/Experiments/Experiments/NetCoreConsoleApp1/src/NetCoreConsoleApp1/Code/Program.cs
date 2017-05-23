using System;

using NetCoreClassLibrary1;


namespace NetCoreConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UsefulClass useful = new UsefulClass();
            useful.Value = @"Other value!";

            Console.WriteLine(useful.Value);
        }
    }
}
