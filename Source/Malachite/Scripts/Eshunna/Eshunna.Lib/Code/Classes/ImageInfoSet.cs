using System;


namespace Eshunna.Lib
{
    public class ImageInfoSet
    {
        public ImageInfo SfmImage { get; }
        public ImageInfo WedgeImage { get; }


        public ImageInfoSet(ImageInfo sfmImage, ImageInfo wedgeImage)
        {
            this.SfmImage = sfmImage;
            this.WedgeImage = wedgeImage;
        }
    }
}
