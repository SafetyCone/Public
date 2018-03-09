using System;
using System.Collections.Generic;

using Public.Common.Lib.Visuals;


namespace Public.Common.Lib.Graphics
{
    public static class Lines
    {
        /// <summary>
        /// Get line pixel locations via the integer Bresenham's line algorithm as shown by Wikipedia.
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm (the "Algorithm for integer arithmetic" and "All cases" sections)
        /// This is a low-quality (but faster) implementation as it produces significant asymetries in the standard reticle test.
        /// The asymmetry is due to having the beginning of line behavior at the beginning-point or at the end-point depending on if you switch the beginning and end points (low vs. high).
        /// </remarks>
        public static List<PixelLocation> LineLocationsByBresenhamLowQuality(PixelLocation p1, PixelLocation p2)
        {
            int x0 = p1.X;
            int y0 = p1.Y;
            int x1 = p2.X;
            int y1 = p2.Y;

            var output = Lines.LineLocationsByBresenhamLowQuality(x0, y0, x1, y1);
            return output;
        }

        public static List<PixelLocation> LineLocationsByBresenhamLowQuality(int x0, int y0, int x1, int y1)
        {
            List<PixelLocation> output;
            int absXDiff = Math.Abs(x1 - x0);
            int absYDiff = Math.Abs(y1 - y0);
            if(absYDiff < absXDiff)
            {
                if(x0 > x1)
                {
                    output = Lines.LineLocationsByBresenham_LineLow(x1, y1, x0, y0); // Swap 1 and 2.
                }
                else
                {
                    output = Lines.LineLocationsByBresenham_LineLow(x0, y0, x1, y1);
                }
            }
            else
            {
                if(y0 > y1)
                {
                    output = Lines.LineLocationsByBresenham_LineHigh(x1, y1, x0, y0); // Swap 1 and 2.
                }
                else
                {
                    output = Lines.LineLocationsByBresenham_LineHigh(x0, y0, x1, y1);
                }
            }
            return output;
        }

        private static List<PixelLocation> LineLocationsByBresenham_LineLow(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            int D = 2 * dy - dx;
            int y = y0;

            int numberOfXValues = x1 - x0 + 1;
            var output = new List<PixelLocation>(numberOfXValues);
            for (int iXValue = 0; iXValue < numberOfXValues; iXValue++)
            {
                int x = x0 + iXValue;
                var location = new PixelLocation(x, y);
                output.Add(location);

                if (D > 0)
                {
                    y = y + yi;
                    D -= 2 * dx;
                }
                D += 2 * dy;
            }
            return output;
        }

        // X and Y are switched.
        private static List<PixelLocation> LineLocationsByBresenham_LineHigh(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            int D = 2 * dx - dy;
            int x = x0;

            int numberOfYValues = y1 - y0 + 1;
            var output = new List<PixelLocation>(numberOfYValues);
            for (int iYValue = 0; iYValue < numberOfYValues; iYValue++)
            {
                int y = y0 + iYValue;
                var location = new PixelLocation(x, y);
                output.Add(location);

                if (D > 0)
                {
                    x = x + xi;
                    D -= 2 * dy;
                }
                D += 2 * dx;
            }
            return output;
        }

        /// <summary>
        /// Get line pixel locations.
        /// </summary>
        /// <remarks>
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm (the "Algorithm for integer arithmetic" and "All cases" sections)
        /// This is a high-quality (but slower) implementation that produces no asymmetries since it merely transforms a common line created in octant one to other octants as needed.
        /// The high-quality implementation is slower than the low-quality implementation as it has to transform each output point from octant 1 to the desired octant.
        /// Note, the transformation also transforms output to octant 1, thus there is no speedup in only performing octant 1 line creation.
        /// </remarks>
        public static List<PixelLocation> LineLocations(PixelLocation p1, PixelLocation p2)
        {
            int octant = Lines.DetermineOctant(p1, p2);

            var transformationToOctantOne = Lines.GetTransformationToOctantOne(octant);
            var p1Oct1 = transformationToOctantOne(p1);
            var p2Oct1 = transformationToOctantOne(p2);

            int x0 = p1Oct1.X;
            int y0 = p1Oct1.Y;
            int x1 = p2Oct1.X;
            int y1 = p2Oct1.Y;
            var pixelLocationsOct1 = Lines.LineLocationsByBresenham_LineLow(x0, y0, x1, y1);

            var output = new List<PixelLocation>(pixelLocationsOct1.Count);
            var transformationFromOctantOne = Lines.GetTransformationFromOctantOne(octant);
            foreach (var pixelLocationOct1 in pixelLocationsOct1)
            {
                var pixelLocation = transformationFromOctantOne(pixelLocationOct1);
                output.Add(pixelLocation);
            }
            return output;
        }

        public static List<PixelLocation> LineLocations(int x0, int y0, int x1, int y1)
        {
            var output = Lines.LineLocations(new PixelLocation(x0, y0), new PixelLocation(x1, y1));
            return output;
        }

        private static int DetermineOctant(PixelLocation p1, PixelLocation p2)
        {
            bool startXLessThanEndX = p1.X < p2.X; // False includes vertical lines (equals).
            bool startYLessThanEndY = p1.Y < p2.Y; // False includes horizontal lines (equals).
            bool horizontallyDominant = Math.Abs(p1.Y - p2.Y) < Math.Abs(p1.X - p2.X); // False includes diagonal lines (equals).

            int octant;
            if (startXLessThanEndX) // Quadrants I and IV.
            {
                if (startYLessThanEndY) // Quadrant I.
                {
                    if (horizontallyDominant)
                    {
                        octant = 1;
                    }
                    else
                    {
                        octant = 2; // And diagonal lines.
                    }
                }
                else // Quadrant IV and horizontal lines.
                {
                    if (horizontallyDominant)
                    {
                        octant = 8;
                    }
                    else
                    {
                        octant = 7; // And diagonal lines.
                    }
                }
            }
            else // Quadrants II and III and vertical lines.
            {
                if (startYLessThanEndY) // Quadrant II.
                {
                    if (horizontallyDominant)
                    {
                        octant = 4;
                    }
                    else
                    {
                        octant = 3; // And diagonal lines.
                    }
                }
                else // Quadrant III and vertical lines.
                {
                    if (horizontallyDominant)
                    {
                        octant = 5;
                    }
                    else
                    {
                        octant = 6; // And diagonal lines.
                    }
                }
            }

            return octant;
        }

        private static Func<PixelLocation, PixelLocation> GetTransformationToOctantOne(int octant)
        {
            Func<PixelLocation, PixelLocation> output;
            switch (octant)
            {
                case 1:
                    output = (p) => new PixelLocation(p.X, p.Y);
                    break;
                case 2:
                    output = (p) => new PixelLocation(p.Y, p.X);
                    break;
                case 3:
                    output = (p) => new PixelLocation(p.Y, -p.X);
                    break;
                case 4:
                    output = (p) => new PixelLocation(-p.X, p.Y);
                    break;
                case 5:
                    output = (p) => new PixelLocation(-p.X, -p.Y);
                    break;
                case 6:
                    output = (p) => new PixelLocation(-p.Y, -p.X);
                    break;
                case 7:
                    output = (p) => new PixelLocation(-p.Y, p.X);
                    break;
                case 8:
                    output = (p) => new PixelLocation(p.X, -p.Y);
                    break;
                default:
                    throw new ArgumentException($@"Invalid octant: {octant.ToString()}.", nameof(octant)); // Should never reach here.
            }
            return output;
        }

        private static Func<PixelLocation, PixelLocation> GetTransformationFromOctantOne(int octant)
        {
            Func<PixelLocation, PixelLocation> output;
            switch (octant)
            {
                case 1:
                    output = (p) => new PixelLocation(p.X, p.Y);
                    break;
                case 2:
                    output = (p) => new PixelLocation(p.Y, p.X);
                    break;
                case 3:
                    output = (p) => new PixelLocation(-p.Y, p.X);
                    break;
                case 4:
                    output = (p) => new PixelLocation(-p.X, p.Y);
                    break;
                case 5:
                    output = (p) => new PixelLocation(-p.X, -p.Y);
                    break;
                case 6:
                    output = (p) => new PixelLocation(-p.Y, -p.X);
                    break;
                case 7:
                    output = (p) => new PixelLocation(p.Y, -p.X);
                    break;
                case 8:
                    output = (p) => new PixelLocation(p.X, -p.Y);
                    break;
                default:
                    throw new ArgumentException($@"Invalid octant: {octant.ToString()}.", nameof(octant)); // Should never reach here.
            }
            return output;
        }
    }
}
