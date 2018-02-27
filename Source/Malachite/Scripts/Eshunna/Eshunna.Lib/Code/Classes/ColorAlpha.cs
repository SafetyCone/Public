using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct ColorAlpha : IEquatable<ColorAlpha>
    {
        #region Static

        public static bool operator ==(ColorAlpha lhs, ColorAlpha rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(ColorAlpha lhs, ColorAlpha rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }


        public ColorAlpha(byte red, byte green, byte blue, byte alpha)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        public ColorAlpha(byte red, byte green, byte blue)
            : this(red, green, blue, Byte.MaxValue)
        {
        }

        public bool Equals(ColorAlpha other)
        {
            bool output =
                this.Red == other.Red &&
                this.Green == other.Green &&
                this.Blue == other.Blue &&
                this.Alpha == other.Alpha;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is ColorAlpha objAsColorAlpha)
            {
                output = this.Equals(objAsColorAlpha);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.Red, this.Green, this.Blue, this.Alpha);
            return output;
        }

        public override string ToString()
        {
            string output = $@"R: {this.Red.ToString()}, G: {this.Green.ToString()}, B: {this.Blue.ToString()}, A: {this.Alpha.ToString()}";
            return output;
        }
    }
}
