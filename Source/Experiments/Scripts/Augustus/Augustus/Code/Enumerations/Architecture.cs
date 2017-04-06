using System;
using Public.Common.Lib;


namespace Augustus
{
    /// <summary>
    /// The 32- or 64- bit architecture for which to build.
    /// </summary>
    /// <remarks>
    /// If you want to build both architectures, include two lines.
    /// </remarks>
    public enum Architecture
    {
        /// <summary>
        /// Whatever is the default for the platform.
        /// </summary>
        Default,
        /// <summary>
        /// For 32 bits.
        /// </summary>
        x86,
        /// <summary>
        /// For 64 bits.
        /// </summary>
        x64
    }
}


namespace Augustus.Extensions
{
    public static class ArchitectureExtensions
    {
        public const string Default = @"Deafult";
        public const string x86 = @"x86";
        public const string x64 = @"Cygwin";


        public static string ToDefaultString(this Architecture Architecture)
        {
            string output;
            switch (Architecture)
            {
                case Architecture.Default:
                    output = ArchitectureExtensions.Default;
                    break;

                case Architecture.x86:
                    output = ArchitectureExtensions.x86;
                    break;

                case Architecture.x64:
                    output = ArchitectureExtensions.x64;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Architecture>(Architecture);
            }

            return output;
        }

        public static Architecture FromDefault(string architecture)
        {
            Architecture output;
            if(!ArchitectureExtensions.TryFromDefault(architecture, out output))
            {
                throw new ArgumentException(@"Unrecognized build architecture string.", nameof(Architecture));
            }

            return output;
        }

        public static bool TryFromDefault(string architecture, out Architecture value)
        {
            bool output = true;
            switch (architecture)
            {
                case ArchitectureExtensions.Default:
                    value = Architecture.Default;
                    break;

                case ArchitectureExtensions.x86:
                    value = Architecture.x86;
                    break;

                case ArchitectureExtensions.x64:
                    value = Architecture.x64;
                    break;

                default:
                    output = false;
                    value = Architecture.Default;
                    break;
            }

            return output;
        }
    }
}