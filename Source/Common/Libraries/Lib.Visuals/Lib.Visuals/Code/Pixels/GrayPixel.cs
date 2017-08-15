

namespace Minex.Common.Lib.Visuals
{
    public class GrayPixel : PixelBase
    {
        public GrayColor Gray { get; set; }


        public GrayPixel() : base() { }

        public GrayPixel(Coordinate coordinate, GrayColor grayColor)
            : base(coordinate)
        {
            this.Gray = grayColor;
        }
    }
}
