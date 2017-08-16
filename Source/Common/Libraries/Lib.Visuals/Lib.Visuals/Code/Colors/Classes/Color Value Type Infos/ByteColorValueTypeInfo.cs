using System;


namespace Public.Common.Lib.Visuals
{
    public class ByteColorValueTypeInfo : IColorValueTypeInfo<byte>
    {
        #region Static

        public static readonly ByteColorValueTypeInfo Instance = new ByteColorValueTypeInfo();

        #endregion


        public byte Max
        {
            get
            {
                return Byte.MaxValue;
            }
        }
        public byte Min
        {
            get
            {
                return Byte.MinValue;
            }
        }
        public byte Range
        {
            get
            {
                return Byte.MaxValue; // Since the min value is zero, the range is the max value.
            }
        }


        /// <summary>
        /// Allows out of range exceptions.
        /// </summary>
        public byte FromDouble(double value)
        {
            byte output = Convert.ToByte(value);
            return output;
        }

        public double ToDouble(byte value)
        {
            double output = (double)value;
            return output;
        }

        /// <summary>
        /// Allows out of range exceptions.
        /// </summary>
        public byte Subtract(byte a, byte b)
        {
            int intermediate = a - b;

            byte output = (byte)intermediate;
            return output;
        }
    }
}