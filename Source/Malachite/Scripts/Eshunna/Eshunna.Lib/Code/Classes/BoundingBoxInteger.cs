using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct BoundingBoxInteger : IEquatable<BoundingBoxInteger>
    {
        #region Static

        public static bool operator ==(BoundingBoxInteger lhs, BoundingBoxInteger rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(BoundingBoxInteger lhs, BoundingBoxInteger rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public int XMin { get; }
        public int XMax { get; }
        public int YMin { get; }
        public int YMax { get; }


        public BoundingBoxInteger(int xMin, int xMax, int yMin, int yMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
        }

        public bool Equals(BoundingBoxInteger other)
        {
            bool output =
                this.XMin == other.XMin &&
                this.XMax == other.XMax &&
                this.YMin == other.YMin &&
                this.YMax == other.YMax;
            return output;
        }

        public override bool Equals(object obj)
        {
            bool output = false;
            if (obj is BoundingBoxInteger objAsBoundingBoxInteger)
            {
                output = this.Equals(objAsBoundingBoxInteger);
            }
            return output;
        }

        public override int GetHashCode()
        {
            int output = HashHelper.GetHashCode(this.XMin, this.XMax, this.YMin, this.YMax);
            return output;
        }

        public override string ToString()
        {
            string output = $@"XMin: {this.XMin.ToString()}, XMax: {this.XMax.ToString()}, YMin: {this.YMin.ToString()}, YMax: {this.YMax.ToString()} (int)";
            return output;
        }
    }


    public static class BoundingBoxIntegerExtensions
    {
        /// <summary>
        /// Assumes that X is the width direction, and Y is the height direction. The output rectangle is assumed be its own objects, thus has X = 0, and Y = 0.
        /// </summary>
        public static RectangleInteger RectangleXWidthGet(this BoundingBoxInteger boundingBox)
        {
            int width = boundingBox.XMax - boundingBox.XMin + 1; // +1 for both end-points inclusive.
            int height = boundingBox.YMax - boundingBox.YMin + 1; // +1 for both end-points inclusive.
            var output = new RectangleInteger(0, 0, width, height);
            return output;
        }
    }
}
