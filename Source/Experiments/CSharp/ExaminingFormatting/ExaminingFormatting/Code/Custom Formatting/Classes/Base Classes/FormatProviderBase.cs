using System;


namespace ExaminingFormatting
{
    /// <summary>
    /// Determines whether the input <see cref="Type"/> instance describes the <see cref="ICustomFormatter"/> interface.
    /// If it does, returns an instance implementing <see cref="ICustomFormatter"/>.
    /// If it doesn't, returns null.
    /// This behavior is described here: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-define-and-use-custom-numeric-format-providers#to-define-a-custom-format-provider
    /// </summary>
    public abstract class FormatProviderBase : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            if(formatType == typeof(ICustomFormatter))
            {
                var output = this.GetCustomFormatter();
                return output;
            }

            return null;
        }

        protected abstract ICustomFormatter GetCustomFormatter();
    }
}
