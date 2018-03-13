using System;
using System.IO;


namespace Eshunna.Lib
{
    /// <summary>
    /// An SFM image file path to wedge file path map that assumes the file name is the same between the SFM and wedge images, but that the wedge image exists in a different directory.
    /// Also assumes that all wedge images are together in the same directory.
    /// </summary>
    public class DifferentDirectorySameFileName : ISfmToWedgeImageFilePathMap
    {
        #region Static

        public static string WedgeImageFilePathGet(string sfmImageFilePath, string wedgeImagesDirectoryPath)
        {
            string sfmImageFileName = Path.GetFileName(sfmImageFilePath);
            string output = Path.Combine(wedgeImagesDirectoryPath, sfmImageFileName);
            return output;
        }

        #endregion


        public string WedgeImagesDirectoryPath { get; }


        public DifferentDirectorySameFileName(string wedgeImagesDirectoryPath)
        {
            this.WedgeImagesDirectoryPath = wedgeImagesDirectoryPath;
        }

        public string WedgeImageFilePath(string sfmImageFilePath)
        {
            string output = DifferentDirectorySameFileName.WedgeImageFilePathGet(sfmImageFilePath, this.WedgeImagesDirectoryPath);
            return output;
        }
    }
}
