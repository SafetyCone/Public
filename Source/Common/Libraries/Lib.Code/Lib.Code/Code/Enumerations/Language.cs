using System;


namespace Public.Common.Lib.Code
{
    // Ok.
    public enum Language
    {
        CSharp,
        Cpp
    }


    public static class LanguageExtensions
    {
        public const string CSharp = @"CSharp";
        public const string Cpp = @"Cpp";


        public static string ToDefaultString(this Language language)
        {
            string output;
            switch (language)
            {
                case Language.CSharp:
                    output = LanguageExtensions.CSharp;
                    break;

                case Language.Cpp:
                    output = LanguageExtensions.Cpp;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Language>(language);
            }

            return output;
        }

        public static Language FromDefault(string language)
        {
            Language output;
            if (!LanguageExtensions.TryFromDefault(language, out output))
            {
                string message = String.Format(@"Unrecognized language string: {0}.", language);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string language, out Language value)
        {
            bool output = true;
            value = Language.CSharp;

            switch (language)
            {
                case LanguageExtensions.CSharp:
                    value = Language.CSharp;
                    break;

                case LanguageExtensions.Cpp:
                    value = Language.Cpp;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
