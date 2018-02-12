

namespace Public.Common.Lib.Visuals
{
    /// <summary>
    /// Provides the size of an image stored as a file in an external format (jpg, bmp, png, etc.)
    /// </summary>
    public interface IExternalFormatImageSizeProvider
    {
        ImageSize GetSize(string externalFormatImageFilePath);
    }
}
