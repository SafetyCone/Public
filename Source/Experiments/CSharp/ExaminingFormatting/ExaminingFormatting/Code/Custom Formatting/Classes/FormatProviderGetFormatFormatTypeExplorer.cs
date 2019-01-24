using System;


namespace ExaminingFormatting
{
    public class FormatProviderGetFormatFormatTypeExplorer : IFormatProvider
    {
        public Func<Type, object> ExplorerFunction { get; set; }


        public object GetFormat(Type formatType)
        {
            var output = this.ExplorerFunction(formatType);
            return output;
        }
    }
}
