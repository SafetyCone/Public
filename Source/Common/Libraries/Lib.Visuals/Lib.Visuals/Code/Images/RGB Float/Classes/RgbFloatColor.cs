using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Represents an RGB color.
    /// 
    /// As a properly implemented structure, this is immutable and implements IEquatable.
    /// </summary>
    public struct RgbFloatColor : IEquatable<RgbFloatColor>
    {
        public const float MaxChannelValue = 1.0f;
        public const float MinChannelValue = 0.0f;
        public const int NumberOfRgbColorChannels = 3;


        #region Static

        public static RgbFloatColor Invalid = new RgbFloatColor(Single.MinValue, Single.MinValue, Single.MinValue);
        public static RgbFloatColor Black => new RgbFloatColor(RgbFloatColor.MinChannelValue, RgbFloatColor.MinChannelValue, RgbFloatColor.MinChannelValue);
        public static RgbFloatColor White => new RgbFloatColor(RgbFloatColor.MaxChannelValue, RgbFloatColor.MaxChannelValue, RgbFloatColor.MaxChannelValue);
        public static RgbFloatColor Red => new RgbFloatColor(RgbFloatColor.MaxChannelValue, RgbFloatColor.MinChannelValue, RgbFloatColor.MinChannelValue);
        public static RgbFloatColor Green => new RgbFloatColor(RgbFloatColor.MinChannelValue, RgbFloatColor.MaxChannelValue, RgbFloatColor.MinChannelValue);
        public static RgbFloatColor Blue => new RgbFloatColor(RgbFloatColor.MinChannelValue, RgbFloatColor.MinChannelValue, RgbFloatColor.MaxChannelValue);


        public static bool operator ==(RgbFloatColor lhs, RgbFloatColor rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(RgbFloatColor lhs, RgbFloatColor rhs)
        {
            return !(lhs.Equals(rhs));
        }

        public static bool IsValid(RgbFloatColor color)
        {
            bool output = color.R != Single.MinValue;
            return output;
        }

        #endregion


        public float R { get; }
        public float G { get; }
        public float B { get; }


        public RgbFloatColor(float red, float green, float blue)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
        }

        public bool Equals(RgbFloatColor other)
        {
            bool output = 
                this.R == other.R && 
                this.G == other.G && 
                this.B == other.B;
            return output;
        }

        public override bool Equals(object obj)
        {
            // Note that because structs are implicitly sealed (no descendent classes) instead of using 'as' we can use 'is'.
            bool output = false;
            if (obj is RgbFloatColor)
            {
                output = this.Equals((RgbFloatColor)obj);
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.R, this.G, this.B);
        }
    }
}
