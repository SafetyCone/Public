using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Visuals.IO.Serialization
{
    public interface IRgbFloatImageExternalFormatSerializer : IInstrumentedFileSerializer<RgbFloatImage>
    {
        ImageFormat[] SupportedExternalFormats { get; }
    }
}
