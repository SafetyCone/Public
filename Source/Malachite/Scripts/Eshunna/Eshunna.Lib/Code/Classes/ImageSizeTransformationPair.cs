using System;

using Public.Common.Lib.Visuals;


namespace Eshunna.Lib
{
    public class ImageSizeTransformationPair
    {
        public ImageSize From { get; }
        public ImageSize To { get; }


        public ImageSizeTransformationPair(ImageSize from, ImageSize to)
        {
            this.From = from;
            this.To = to;
        }
    }
}
