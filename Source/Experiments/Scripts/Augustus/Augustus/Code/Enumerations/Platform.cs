﻿using System;
using Public.Common.Lib;


namespace Augustus
{
    /// <summary>
    /// The 32- or 64- bit platform for which to build.
    /// </summary>
    /// <remarks>
    /// If you want to build both platforms, copy-and-paste two lines in the build list.
    /// </remarks>
    public enum Platform
    {
        /// <summary>
        /// Whatever is the default for the operating system environment.
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
    public static class PlatformExtensions
    {
        public const string Default = @"Deafult";
        public const string x86 = @"x86";
        public const string x64 = @"Cygwin";


        public static string ToDefaultString(this Platform Architecture)
        {
            string output;
            switch (Architecture)
            {
                case Platform.Default:
                    output = PlatformExtensions.Default;
                    break;

                case Platform.x86:
                    output = PlatformExtensions.x86;
                    break;

                case Platform.x64:
                    output = PlatformExtensions.x64;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(Architecture);
            }

            return output;
        }

        public static Platform FromDefault(string architecture)
        {
            Platform output;
            if(!PlatformExtensions.TryFromDefault(architecture, out output))
            {
                throw new ArgumentException(@"Unrecognized build architecture string.", nameof(Platform));
            }

            return output;
        }

        public static bool TryFromDefault(string architecture, out Platform value)
        {
            bool output = true;
            switch (architecture)
            {
                case PlatformExtensions.Default:
                    value = Platform.Default;
                    break;

                case PlatformExtensions.x86:
                    value = Platform.x86;
                    break;

                case PlatformExtensions.x64:
                    value = Platform.x64;
                    break;

                default:
                    output = false;
                    value = Platform.Default;
                    break;
            }

            return output;
        }
    }
}