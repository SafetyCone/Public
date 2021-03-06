﻿using System;


namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// The location of a pixel within an image, given by integer pixels. By convention, X is the image width dimension, and Y is the iamge height dimension. X runs across the columns, and Y runs down the rows.
    /// </summary>
    /// <remarks>
    /// * By image convention, X is the width dimention, and Y is the height dimension.
    /// * By image convention, X runs across the columns from left to right, and Y runs down the rows from top to bottom.
    /// * By image convention, X increases to the right, and Y increases downwards.
    /// * No assumption is made about the image origin, although the image convention is to have the origin in the upper-left of the image.
    /// </remarks>
    public struct PixelLocation : IEquatable<PixelLocation>
    {
        #region Static

        public static bool operator ==(PixelLocation lhs, PixelLocation rhs)
        {
            return lhs.Equals(rhs);
        }

        // If op_Equals is present, op_NotEquals must also be present, and vice-versa.
        public static bool operator !=(PixelLocation lhs, PixelLocation rhs)
        {
            return !(lhs.Equals(rhs));
        }

        #endregion


        public int X { get; }
        public int Column => this.X;
        public int Y { get; }
        public int Row => this.Y;


        public PixelLocation(int x_column, int y_row)
        {
            this.X = x_column;
            this.Y = y_row;
        }

        public bool Equals(PixelLocation other)
        {
            bool output =
                this.X == other.X &&
                this.Y == other.Y;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is PixelLocation objAsPixelLocation)
            {
                output = this.Equals(objAsPixelLocation);
            }
            return output;
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.X, this.Y);
        }

        public override string ToString()
        {
            string output = $@"X: {this.X.ToString()}, Y: {this.Y.ToString()} (int)";
            return output;
        }
    }
}
