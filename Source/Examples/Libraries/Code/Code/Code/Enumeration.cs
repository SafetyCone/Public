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
            if(!EnumerationExtensions.TryFromDefault(enumeration, out output))
            {
#if (NETFX_40) // Make sure the Public/Tools/VersionSpecificSymbols.Common.prop import is included in the project file.
                string varName = nameof(output);
                throw new ArgumentException(@"Unrecognized enumeration string.", nameof(enumeration));
#else
                throw new ArgumentException(@"Unrecognized enumeration string.", "enumeration");
#endif
            }

            return output;
        }

        public static bool TryFromDefault(string enumeration, out Enumeration value)
        {
            bool output = true;
            switch (enumeration)
            {
                case EnumerationExtensions.A:
                    value = Enumeration.A;
                    break;

                case EnumerationExtensions.B:
                    value = Enumeration.B;
                    break;

                default:
                    output = false;
                    value = Enumeration.A; // Or other appropriate default.
                    break;
            }

            return output;
        }
    }
}