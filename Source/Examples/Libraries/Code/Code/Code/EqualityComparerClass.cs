using System;
using System.Collections.Generic;

using Public.Common.Lib;


namespace Public.Examples.Code
{
    /// <summary>
    /// Inherit from <see cref="EqualityComparer{T}"/> instead of implementing <see cref="IEqualityComparer{T}"/>, as stated here: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1?view=netframework-4.7.2.
    /// Not sure why...
    /// Use the <see cref="EqualityComparer{T}.Default"/> to provide results for the required abstract overrides, as stated here: https://books.google.com/books?id=qzoM-kBzy2YC&pg=PT27&lpg=PT27&dq=why+derive+from+equalitycomparer&source=bl&ots=kwvoNwXG2s&sig=e9nQ3HlSFi8_HHf_7jXgXPFH0Jg&hl=en&sa=X&ved=2ahUKEwjW3eXp_e7fAhWOmuAKHaI1BcoQ6AEwCHoECAIQAQ#v=onepage&q=why%20derive%20from%20equalitycomparer&f=false
    /// Note, the T type should implement <see cref="IEquatable{T}"/>.
    /// </summary>
    public class EqualityComparerClass : EqualityComparer<ExampleClass>
    {
        public int A { get; set; }
        public string B { get; set; }


        public override bool Equals(ExampleClass x, ExampleClass y)
        {
            return EqualityComparer<ExampleClass>.Default.Equals(x, y);
        }

        public override int GetHashCode(ExampleClass obj)
        {
            return EqualityComparer<ExampleClass>.Default.GetHashCode();
        }
    }
}
