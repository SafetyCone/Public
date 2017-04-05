using System;
using Public.Common.Lib;


namespace Public.Examples.Code
{
    public enum Enumeration
    {
        A,
        B
    }
}


namespace Public.Examples.Code.Extensions
{
    public static class EnumerationExtensions
    {
        public const string A = @"A";
        public const string B = @"B";


        public static string ToDefaultString(this Enumeration enumeration)
        {
            string output;
            switch (enumeration)
            {
                case Enumeration.A:
                    output = EnumerationExtensions.A;
                    break;

                case Enumeration.B:
                    output = EnumerationExtensions.B;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Enumeration>(enumeration);
            }

            return output;
        }

        public static Enumeration FromDefault(string enumeration)
        {
            Enumeration output;
            switch (enumeration)
            {
                case EnumerationExtensions.A:
                    output = Enumeration.A;
                    break;

                case EnumerationExtensions.B:
                    output = Enumeration.B;
                    break;

                default:
                    throw new ArgumentException(@"Unrecognized enumeration string.", nameof(enumeration));
            }

            return output;
        }
    }
}