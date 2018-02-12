using Public.Common.Lib;
using Public.Common.Lib.Visuals;


namespace Opis
{
    public struct CameraCalibrationParameters
    {
        public BoardConfiguration BoardConfiguration { get; }
        public CameraCalibrationEstimationParameters CameraCalibrationEstimationParameters { get; }
        public ImageSize ImageSize { get; }


        public CameraCalibrationParameters(BoardConfiguration boardConfiguration, CameraCalibrationEstimationParameters cameraCalibrationEstimationParameters, ImageSize imageSize)
        {
            this.BoardConfiguration = boardConfiguration;
            this.CameraCalibrationEstimationParameters = cameraCalibrationEstimationParameters;
            this.ImageSize = imageSize;
        }
    }
}
