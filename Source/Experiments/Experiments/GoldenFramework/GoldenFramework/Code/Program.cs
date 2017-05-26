using System;

using Library;


namespace GoldenFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"The answer is {new Thing().Get(42)}.");
        }
    }
}
