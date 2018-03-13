using System;

using Public.Common.Lib.Visuals;


namespace Eshunna.Lib
{
    public class ImageInfo
    {
        public string FilePath { get; }
        public ImageSize Size { get; }


        public ImageInfo(string filePath, ImageSize size)
        {
            this.FilePath = filePath;
            this.Size = size;
        }
    }
}
