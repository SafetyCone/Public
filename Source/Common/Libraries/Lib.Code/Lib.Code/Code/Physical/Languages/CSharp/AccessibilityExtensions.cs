using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    public static class AccessibilityExtensions
    {
        public const string Private = Constants.PrivateKeyword;
        public const string Public = Constants.PublicKeyword;
        public const string Protected = Constants.ProtectedKeyword;


        public static string ToDefaultString(this Accessibility accessibility)
        {
            string output;
            switch (accessibility)
            {
                case Accessibility.Private:
                    output = AccessibilityExtensions.Private;
                    break;

                case Accessibility.Public:
                    output = AccessibilityExtensions.Public;
                    break;

                case Accessibility.Protected:
                    output = AccessibilityExtensions.Protected;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Accessibility>(accessibility);
            }

            return output;
        }

        public static Accessibility FromDefault(string accessibility)
        {
            Accessibility output;
            if (!AccessibilityExtensions.TryFromDefault(accessibility, out output))
            {
                string message = String.Format(@"Unrecognized C# accessibility string: {0}.", accessibility);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string accessibility, out Accessibility value)
        {
            bool output = true;
            value = Accessibility.Private;

            switch (accessibility)
            {
                case AccessibilityExtensions.Private:
                    value = Accessibility.Private;
                    break;

                case AccessibilityExtensions.Public:
                    value = Accessibility.Public;
                    break;

                case AccessibilityExtensions.Protected:
                    value = Accessibility.Protected;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
