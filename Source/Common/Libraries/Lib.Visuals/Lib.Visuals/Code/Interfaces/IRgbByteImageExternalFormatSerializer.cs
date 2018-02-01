using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Visuals.IO.Serialization
{
    /// <summary>
    /// Represents a type that can serialize RGB byte images to and from external image formats like JPG, BMP, PNG, etc...
    /// </summary>
    public interface IRgbByteImageExternalFormatSerializer : IInstrumentedFileSerializer<RgbByteImage>
    {
        ImageFormat[] SupportedExternalFormats { get; }
    }
}
