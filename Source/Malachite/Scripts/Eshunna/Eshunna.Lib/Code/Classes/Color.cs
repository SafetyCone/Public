using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct Color : IEquatable<Color>
    {
        #region Static

        public static bool operator ==(Color lhs, Color rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(Color lhs, Color rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }


        public Color(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public bool Equals(Color other)
        {
            bool output =
                this.Red == other.Red &&
                this.Green == other.Green &&
                this.Blue == other.Blue;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is Color objAsColor)
            {
                output = this.Equals(objAsColor);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.Red, this.Green, this.Blue);
            return output;
        }

        public override string ToString()
        {
            string output = $@"R: {this.Red.ToString()}, G: {this.Green.ToString()}, B: {this.Blue.ToString()}";
            return output;
        }
    }
}
