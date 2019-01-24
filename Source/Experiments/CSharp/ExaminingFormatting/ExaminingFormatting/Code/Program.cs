using System;

namespace ExaminingFormatting
{
    class Program
    {
        static void Main(string[] args)
        {
            var intValue = 3;
            var str = $@"IntValue: {intValue}".ToString();

            //ObjectToStringExperiments.SubMain();
            CustomFormattingExperiments.SubMain();
        }
    }
}
