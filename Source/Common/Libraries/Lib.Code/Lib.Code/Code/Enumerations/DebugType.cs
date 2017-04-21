using System;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Specifies a Visual Studio project build configuration debug type.
    /// </summary>
    public enum DebugType
    {
        Full,
        PdbOnly,
    }


    /// <summary>
    /// For use in reading/writing Visual Studio project files.
    /// </summary>
    public static class DebugTypeExtensions
    {
        public const string Full = @"full";
        public const string PdbOnly = @"pdbonly";


        public static string ToDefaultString(this DebugType debugType)
        {
            string output;
            switch (debugType)
            {
                case DebugType.Full:
                    output = DebugTypeExtensions.Full;
                    break;

                case DebugType.PdbOnly:
                    output = DebugTypeExtensions.PdbOnly;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<DebugType>(debugType);
            }

            return output;
        }

        public static DebugType FromDefault(string debugType)
        {
            DebugType output;
            if (!DebugTypeExtensions.TryFromDefault(debugType, out output))
            {
                string message = String.Format(@"Unrecognized debug type string: {0}.", debugType);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string debugType, out DebugType value)
        {
            bool output = true;
            value = DebugType.Full;

            switch (debugType)
            {
                case DebugTypeExtensions.Full:
                    value = DebugType.Full;
                    break;

                case DebugTypeExtensions.PdbOnly:
                    value = DebugType.PdbOnly;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
