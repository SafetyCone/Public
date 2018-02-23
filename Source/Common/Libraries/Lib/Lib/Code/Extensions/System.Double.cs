using System;


namespace Public.Common.Lib.Extensions
{
    public static class DoubleExtensions
    {
        // https://stackoverflow.com/questions/374316/round-a-double-to-x-significant-figures
        // https://channel9.msdn.com/Forums/Coffeehouse/How-to-format-a-double-to-a-specific-number-of-significant-figures-in-C
        // http://csharphelper.com/blog/2016/07/display-significant-digits-in-c/ (Best)
        public static double RoundToSignificantDigits(this double d, int digits)
        {
            double output;
            if(0 == d)
            {
                output = 0;
            }
            else
            {
                bool negative = d < 0;

                double value = Math.Abs(d);
                double scale = Math.Pow(10, Math.Floor(Math.Log10(value)) + 1);
                double toRound = value / scale;
                double rounded = scale * Math.Round(toRound);
                output = negative ? -rounded : rounded;
            }

            return output;
        }
    }
}
