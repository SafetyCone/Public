using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    /// <summary>
    /// C# keyword representations.
    /// </summary>
    public static class ArgumentPassTypeExtensions
    {
        public const string Normal = @"";
        public const string Reference = @"ref";
        public const string Out = @"out";


        public static string ToDefaultString(this ArgumentPassType argumentPassType)
        {
            string output;
            switch (argumentPassType)
            {
                case ArgumentPassType.Normal:
                    output = ArgumentPassTypeExtensions.Normal;
                    break;

                case ArgumentPassType.Reference:
                    output = ArgumentPassTypeExtensions.Reference;
                    break;

                case ArgumentPassType.Out:
                    output = ArgumentPassTypeExtensions.Out;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ArgumentPassType>(argumentPassType);
            }

            return output;
        }

        public static ArgumentPassType FromDefault(string accessibility)
        {
            ArgumentPassType output;
            if (!ArgumentPassTypeExtensions.TryFromDefault(accessibility, out output))
            {
                string message = String.Format(@"Unrecognized C# argument pass type string: {0}.", accessibility);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string accessibility, out ArgumentPassType projectTypeValue)
        {
            bool output = true;
            projectTypeValue = ArgumentPassType.Normal;

            switch (accessibility)
            {
                case ArgumentPassTypeExtensions.Normal:
                    projectTypeValue = ArgumentPassType.Normal;
                    break;

                case ArgumentPassTypeExtensions.Reference:
                    projectTypeValue = ArgumentPassType.Reference;
                    break;

                case ArgumentPassTypeExtensions.Out:
                    projectTypeValue = ArgumentPassType.Out;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
