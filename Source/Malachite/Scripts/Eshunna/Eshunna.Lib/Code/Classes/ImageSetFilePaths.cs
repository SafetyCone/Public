using System;


namespace Eshunna.Lib
{
    public class ImageSetFilePaths
    {
        public string SfmImageFilePath { get; }
        public string WedgeImageFilePath { get; }


        public ImageSetFilePaths(string sfmImageFilePath, string wedgeImageFilePath)
        {
            this.SfmImageFilePath = sfmImageFilePath;
            this.WedgeImageFilePath = wedgeImageFilePath;
        }
    }
}
