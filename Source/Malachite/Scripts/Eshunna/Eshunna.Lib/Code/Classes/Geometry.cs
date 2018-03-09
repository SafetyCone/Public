using System;
using System.Collections.Generic;

using Public.Common.Lib.Graphics;
using Public.Common.Lib.Visuals;


namespace Eshunna.Lib
{
    public static class Geometry
    {
        public static double Sign(Location2Double point, Location2Double u, Location2Double v)
        {
            double output = (point.X - v.X) * (u.Y - v.Y) - (point.Y - v.Y) * (u.X - v.X);
            return output;
        }

        public static float Sign(Location2Float point, Location2Float u, Location2Float v)
        {
            float output = (point.X - v.X) * (u.Y - v.Y) - (point.Y - v.Y) * (u.X - v.X);
            return output;
        }

        public static int Sign(Location2Integer point, Location2Integer u, Location2Integer v)
        {
            int output = (point.X - v.X) * (u.Y - v.Y) - (point.Y - v.Y) * (u.X - v.X);
            return output;
        }

        public static bool IsPointInTriangle(Location2Double point, Location2Double v1, Location2Double v2, Location2Double v3)
        {
            // Handles both right- and left-handed triangles test of equality among all three tests.
            double tolerance = 0d;
            bool b1 = Geometry.Sign(point, v1, v2) < tolerance;
            bool b2 = Geometry.Sign(point, v2, v3) < tolerance;
            bool b3 = Geometry.Sign(point, v3, v1) < tolerance;

            bool output = ((b1 == b2) && (b2 == b3));
            return output;
        }

        public static bool IsPointInTriangle(Location2Float point, Location2Float v1, Location2Float v2, Location2Float v3)
        {
            // Handles both right- and left-handed triangles test of equality among all three tests.
            float tolerance = 0f;
            bool b1 = Geometry.Sign(point, v1, v2) < tolerance;
            bool b2 = Geometry.Sign(point, v2, v3) < tolerance;
            bool b3 = Geometry.Sign(point, v3, v1) < tolerance;

            bool output = ((b1 == b2) && (b2 == b3));
            return output;
        }

        public static bool IsPointInTriangle(Location2Integer point, Location2Integer v1, Location2Integer v2, Location2Integer v3)
        {
            // Handles both right- and left-handed triangles test of equality among all three tests.
            int tolerance = 0;
            bool b1 = Geometry.Sign(point, v1, v2) < tolerance;
            bool b2 = Geometry.Sign(point, v2, v3) < tolerance;
            bool b3 = Geometry.Sign(point, v3, v1) < tolerance;

            bool output = ((b1 == b2) && (b2 == b3));
            return output;
        }

        /// <summary>
        /// Returns a list of all integer positions lying within a triangle.
        /// </summary>
        /// <remarks>
        /// Output X, Y coordinates assume X is the column, and Y is the Row.
        /// </remarks>
        public static List<Location2Integer> ListLocationsInTriangle(Location2Double v1, Location2Double v2, Location2Double v3)
        {
            double xMinVertex = Math.Min(v1.X, Math.Min(v2.X, v3.X));
            double xMaxVertex = Math.Max(v1.X, Math.Max(v2.X, v3.X));
            double yMinVertex = Math.Min(v1.Y, Math.Min(v2.Y, v3.Y));
            double yMaxVertex = Math.Max(v1.Y, Math.Max(v2.Y, v3.Y));

            int xMinBounding = Convert.ToInt32(Math.Floor(xMinVertex));
            int xMaxBounding = Convert.ToInt32(Math.Ceiling(xMaxVertex));
            int yMinBounding = Convert.ToInt32(Math.Floor(yMinVertex));
            int yMaxBounding = Convert.ToInt32(Math.Ceiling(yMaxVertex));

            int width = xMaxBounding - xMinBounding + 1;
            int height = yMaxBounding - yMinBounding + 1;

            // Even though the X, and Y handling suggests an origin at the upper left, increasing to the right and down, the output locations are just indices that remain to be interpreted.
            var output = new List<Location2Integer>();
            for (int iRow = 0; iRow < height; iRow++)
            {
                for (int iCol = 0; iCol < width; iCol++)
                {
                    Location2Double point = new Location2Double(xMinBounding + iCol, yMinBounding + iRow);
                    bool isPointInTriangle = Geometry.IsPointInTriangle(point, v1, v2, v3);
                    if(isPointInTriangle)
                    {
                        output.Add(new Location2Integer(xMinBounding + iCol, yMinBounding + iRow));
                    }
                }
            }
            return output;
        }

        public static List<Location2Integer> ListLocationsInTriangle(Location2Integer v1, Location2Integer v2, Location2Integer v3)
        {
            int xMinVertex = Math.Min(v1.X, Math.Min(v2.X, v3.X));
            int xMaxVertex = Math.Max(v1.X, Math.Max(v2.X, v3.X));
            int yMinVertex = Math.Min(v1.Y, Math.Min(v2.Y, v3.Y));
            int yMaxVertex = Math.Max(v1.Y, Math.Max(v2.Y, v3.Y));

            int width = xMaxVertex - xMinVertex + 1;
            int height = yMaxVertex - yMinVertex + 1;

            // Even though the X, and Y handling suggests an origin at the upper left, increasing to the right and down, the output locations are just indices that remain to be interpreted.
            var output = new List<Location2Integer>();
            for (int iRow = 0; iRow < height; iRow++)
            {
                for (int iCol = 0; iCol < width; iCol++)
                {
                    Location2Integer point = new Location2Integer(xMinVertex + iCol, yMinVertex + iRow);
                    bool isPointInTriangle = Geometry.IsPointInTriangle(point, v1, v2, v3);
                    if (isPointInTriangle)
                    {
                        output.Add(new Location2Integer(xMinVertex + iCol, yMinVertex + iRow));
                    }
                }
            }
            return output;
        }

        public static List<Location2Integer> ListTriangleLocations(Location2Integer v1, Location2Integer v2, Location2Integer v3)
        {
            var locationsInTriangle = Geometry.ListLocationsInTriangle(v1, v2, v3);

            var v1PixelLocation = new PixelLocation(v1.X, v1.Y);
            var v2PixelLocation = new PixelLocation(v2.X, v2.Y);
            var v3PixelLocation = new PixelLocation(v3.X, v3.Y);
            var v1v2PixelLocations = Lines.LineLocations(v1PixelLocation, v2PixelLocation);
            var v2v3PixelLocations = Lines.LineLocations(v2PixelLocation, v3PixelLocation);
            var v3v1PixelLocations = Lines.LineLocations(v3PixelLocation, v1PixelLocation);

            var uniqueLocations = new HashSet<Location2Integer>(locationsInTriangle);
            v1v2PixelLocations.ForEach((p) => uniqueLocations.Add(new Location2Integer(p.X, p.Y)));
            v1v2PixelLocations.ForEach((p) => uniqueLocations.Add(new Location2Integer(p.X, p.Y)));
            v3v1PixelLocations.ForEach((p) => uniqueLocations.Add(new Location2Integer(p.X, p.Y)));

            var output = new List<Location2Integer>(uniqueLocations);
            return output;
        }
    }
}
