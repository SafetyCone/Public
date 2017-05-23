using System;

using NetCoreClassLibrary1;


namespace NetCoreConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UsefulClass useful = new UsefulClass();
            Console.WriteLine(useful.Value);

            useful.Value = @"Other value!";
            Console.WriteLine(useful.Value);
        }
    }
}
