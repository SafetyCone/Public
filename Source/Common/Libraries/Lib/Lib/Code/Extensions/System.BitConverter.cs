﻿using System;


namespace Public.Common.Lib.Extensions
{
    public static class BitConverterExtensions
    {
        /// <summary>
        /// Creates a 32-bit integer (the C# default int) from only two bytes. Similar to System.BitConverter.ToInt32(), but requires providing only two bytes.
        /// </summary>
        public static int ToInt32FromTwoBytes(byte[] buffer, int offset)
        {
            int output = buffer[offset] + (buffer[offset + 1] << 8);
            return output;
        }

        /// <summary>
        /// Creates a 32-bit integer (the C# default int) from only two bytes. Similar to System.BitConverter.ToInt32(), but requires providing only two bytes.
        /// </summary>
        public static byte[] ToTwoBytesFromInt32(int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);

            byte[] output = new byte[] { buffer[0], buffer[1] };
            return output;
        }
    }
}
