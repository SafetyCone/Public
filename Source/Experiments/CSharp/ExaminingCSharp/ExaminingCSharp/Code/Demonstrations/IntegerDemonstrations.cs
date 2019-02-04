using System;


namespace ExaminingCSharp
{
    public static class IntegerDemonstrations
    {
        public static void SubMain()
        {
            IntegerDemonstrations.IntegerDivisionRoundsDown();
        }

        /// <summary>
        /// Result: As expected, integer division rounds down.
        /// When two integers are divided, only in special cases is the result an integer.
        /// However, in C#, the result of integer division is indeed an integer. The question arises as to how integer division in C# decides rounder. Does integer division round-down (flooring), or round-up (ceiling).
        /// Expected: Integer division is the same as the floor operation since underflowed bits are just dropped.
        /// 
        /// As stated here, rounds towards zero: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/division-operator
        /// </summary>
        public static void IntegerDivisionRoundsDown()
        {
            int a = 4;
            int b = 3;

            int c = a / b; // 1
            Console.WriteLine($@"{a} / {b} = {c}");

            int d = b / a; // 0
            Console.WriteLine($@"{b} / {a} = {d}");
        }
    }
}
