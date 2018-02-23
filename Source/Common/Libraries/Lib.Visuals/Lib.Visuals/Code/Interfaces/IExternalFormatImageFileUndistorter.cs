

namespace Public.Common.Lib.Visuals
{
    public interface IExternalFormatImageFileUndistorter
    {
        void Undistort(string distortedImageFilePath, string undistortedImageFilePath);
    }
}
