using System;


namespace Eshunna.Lib.SIFT
{
    public class SiftFeature
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Scale { get; set; }
        public float Orientation { get; set; }
        public ColorAlpha Color { get; set; }


        public SiftFeature(float x, float y, float scale, float orientation, ColorAlpha color)
        {
            this.X = x;
            this.Y = y;
            this.Scale = scale;
            this.Orientation = orientation;
            this.Color = color;
        }
    }
}
