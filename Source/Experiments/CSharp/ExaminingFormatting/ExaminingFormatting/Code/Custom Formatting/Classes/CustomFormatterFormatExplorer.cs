using System;


namespace ExaminingFormatting
{
    public class CustomFormatterFormatExplorer : ICustomFormatter
    {
        public Func<string, object, IFormatProvider, string> ExplorerFunction { get; set; }


        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var output = this.ExplorerFunction(format, arg, formatProvider);
            return output;
        }
    }
}
