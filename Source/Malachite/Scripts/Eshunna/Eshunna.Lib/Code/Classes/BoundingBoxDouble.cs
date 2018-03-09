using System;

using Public.Common.Lib;


namespace Eshunna.Lib
{
    [Serializable]
    public struct BoundingBoxDouble : IEquatable<BoundingBoxDouble>
    {
        #region Static

        public static bool operator ==(BoundingBoxDouble lhs, BoundingBoxDouble rhs)
        {
            bool output = lhs.Equals(rhs);
            return output;
        }

        public static bool operator !=(BoundingBoxDouble lhs, BoundingBoxDouble rhs)
        {
            bool output = !lhs.Equals(rhs);
            return output;
        }

        #endregion


        public double XMin { get; }
        public double XMax { get; }
        public double YMin { get; }
        public double YMax { get; }


        public BoundingBoxDouble(double xMin, double xMax, double yMin, double yMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
        }

        public bool Equals(BoundingBoxDouble other)
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
            if (obj is BoundingBoxDouble objAsBoundingBoxDouble)
            {
                output = this.Equals(objAsBoundingBoxDouble);
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
            string output = $@"XMin: {this.XMin.ToString()}, XMax: {this.XMax.ToString()}, YMin: {this.YMin.ToString()}, YMax: {this.YMax.ToString()} (double)";
            return output;
        }
    }


    public static class BoundingBoxDoubleExtensions
    {
        public static RectangleDouble Rectangle(this BoundingBoxDouble boundingBox)
        {
            double width = boundingBox.XMax - boundingBox.XMin;
            double height = boundingBox.YMax - boundingBox.YMin;
            var output = new RectangleDouble(0, 0, width, height);
            return output;
        }

        public static BoundingBoxInteger ToBoundingBoxInteger(this BoundingBoxDouble boundingBox)
        {
            int xMin = Convert.ToInt32(Math.Floor(boundingBox.XMin));
            int xMax = Convert.ToInt32(Math.Ceiling(boundingBox.XMax));
            int yMin = Convert.ToInt32(Math.Floor(boundingBox.YMin));
            int yMax = Convert.ToInt32(Math.Ceiling(boundingBox.YMax));

            var output = new BoundingBoxInteger(xMin, xMax, yMin, yMax);
            return output;
        }
    }
}
