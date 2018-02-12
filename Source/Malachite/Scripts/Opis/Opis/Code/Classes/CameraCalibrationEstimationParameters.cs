

namespace Opis
{
    public struct CameraCalibrationEstimationParameters
    {
        public bool EstimateSkew { get; }
        public bool EstimateTangentialDistortion { get; }
        public RadialDistortionCoefficientCount RadialDistortionCoefficientCount { get; }


        public CameraCalibrationEstimationParameters(bool estimateSkew, bool estimateTangentialDistortion, RadialDistortionCoefficientCount radialDistortionCoefficientCount)
        {
            this.EstimateSkew = estimateSkew;
            this.EstimateTangentialDistortion = estimateTangentialDistortion;
            this.RadialDistortionCoefficientCount = radialDistortionCoefficientCount;
        }
    }
}
