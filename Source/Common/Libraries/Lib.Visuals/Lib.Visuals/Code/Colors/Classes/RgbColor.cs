using System;

using Public.Common.Lib;


namespace Public.Common.Lib.Visuals
{
    public struct RgbColor
    {
        public const int NumberOfRgbColorChannels = 3;
        public const int RedChannelIndex = 0;
        public const int GreenChannelIndex = 1;
        public const int BlueChannelIndex = 2;
    }


    /// <summary>
    /// Represents a color specified by RGB values, allowing a generic type for the color channel values.
    /// </summary>
    /// <remarks>
    /// An immutable structure, 
    /// </remarks>
    [Serializable]
    public struct RgbColor<T> : IEquatable<RgbColor<T>>
        where T: struct, IEquatable<T> // The IEquatable restriction on T is required to prevent boxing in the IEquatable.Equals() method for RgbColor.
    {
        #region Static

        public static bool operator ==(RgbColor<T> lhs, RgbColor<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(RgbColor<T> lhs, RgbColor<T> rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion

        #region IEquatable Members

        public bool Equals(RgbColor<T> other)
        {
            // Use of Equals() method with IEquatable constraint on type parameter prevents boxing.
            // Confirmed using ILSpy and looking at the generated IL, the IEquatable constraint is required.
            bool output = (this.zRed.Equals(other.zRed)) && (this.zGreen.Equals(other.zGreen)) && (this.zBlue.Equals(other.zBlue));
            return output;
        }

        #endregion


        private readonly T zRed;
        public T Red
        {
            get
            {
                return this.zRed;
            }
        }
        private readonly T zGreen;
        public T Green
        {
            get
            {
                return this.zGreen;
            }
        }
        private readonly T zBlue;
        public T Blue
        {
            get
            {
                return this.zBlue;
            }
        }


        public RgbColor(T red, T green, T blue)
        {
            this.zRed = red;
            this.zGreen = green;
            this.zBlue = blue;
        }

        public override bool Equals(object obj)
        {
            if (obj is RgbColor<T>)
            {
                return this.Equals((RgbColor<T>)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.zRed, this.zGreen, this.zBlue);
        }
    }
}
