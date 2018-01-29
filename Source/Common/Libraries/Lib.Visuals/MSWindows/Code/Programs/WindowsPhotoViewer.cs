using System;
using System.Drawing;
using SysImageFormat = System.Drawing.Imaging.ImageFormat;
using System.IO;

using Public.Common.Lib.IO.Extensions;


namespace Public.Common.Lib.Visuals.MSWindows
{
    /// <summary>
    /// Uses the Windows Photo Viewer.
    /// </summary>
    public class WindowsPhotoViewer
    {
        #region Static

        public static string DefaultTemporaryImageFilesDirectoryPath { get; } = @"E:\temp\Images";


        private static string GetTemporaryImageFilePath()
        {
            string temporaryImageFilesDirectoryPath = WindowsPhotoViewer.DefaultTemporaryImageFilesDirectoryPath;
            if (!Directory.Exists(temporaryImageFilesDirectoryPath))
            {
                Directory.CreateDirectory(temporaryImageFilesDirectoryPath);
            }

            Guid temporaryImageGuid = Guid.NewGuid();
            string temporaryImageFileName = $@"{temporaryImageGuid.ToString()}{PathExtensions.WindowsFileExtensionSeparatorChar}{FileExtensions.DefaultImageFileExtension}";
            string temporaryImageFilePath = Path.Combine(temporaryImageFilesDirectoryPath, temporaryImageFileName);
            return temporaryImageFilePath;
        }

        public static void View(string imageFilePath)
        {
            string rundll32PathMask = @"%SystemRoot%\System32\rundll32.exe";
            string rundll32Path = rundll32PathMask.Replace(@"%SystemRoot%", Environment.ExpandEnvironmentVariables(@"%SystemRoot%"));

            string photoViewerDllPathMask = @"""%ProgramFiles%\Windows Photo Viewer\PhotoViewer.dll"", ImageView_Fullscreen {0}";
            string photoViewerDllPath = photoViewerDllPathMask.Replace(@"%ProgramFiles%", Environment.ExpandEnvironmentVariables(@"%ProgramFiles%"));

            string arguments = String.Format(photoViewerDllPath, imageFilePath);
            ProcessStarter.StartProcess(rundll32Path, arguments);
        }

        public static void View(RgbByteImage rgbByteImage, string imageFilePath)
        {
            // Convert to bitmap.
            Bitmap bitmap = rgbByteImage.ToBitmap();

            // Save the bitmap to the image file path.
            SysImageFormat imageFormat = FileExtensions.DetermineImageFormatFromFileExtension(imageFilePath);
            bitmap.Save(imageFilePath, imageFormat);

            // Show in Windows Photo Viewer.
            WindowsPhotoViewer.View(imageFilePath);
        }

        public static void View(RgbByteImage rgbByteImage)
        {
            // Get a temporary file.
            string temporaryImageFilePath = WindowsPhotoViewer.GetTemporaryImageFilePath();

            WindowsPhotoViewer.View(rgbByteImage, temporaryImageFilePath);
        }

        public static void View(RgbFloatImage rgbFloatImage, string imageFilePath)
        {
            // Convert to bitmap.
            Bitmap bitmap = rgbFloatImage.ToBitmap();

            // Save the bitmap to the image file path.
            SysImageFormat imageFormat = FileExtensions.DetermineImageFormatFromFileExtension(imageFilePath);
            bitmap.Save(imageFilePath, imageFormat);

            // Show in Windows Photo Viewer.
            WindowsPhotoViewer.View(imageFilePath);
        }

        public static void View(RgbFloatImage rgbFloatImage)
        {
            // Get a temporary file.
            string temporaryImageFilePath = WindowsPhotoViewer.GetTemporaryImageFilePath();

            WindowsPhotoViewer.View(rgbFloatImage, temporaryImageFilePath);
        }

        #endregion
    }
}
