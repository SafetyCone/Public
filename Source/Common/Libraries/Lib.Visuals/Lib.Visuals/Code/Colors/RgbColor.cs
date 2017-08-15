using System;
using System.Drawing;


namespace Minex.Common.Lib.Visuals
{
    /// <summary>
    /// Represents a color specified by RGB values. All RGB values should be in the range [0, 1].
    /// </summary>
    [Serializable]
    public class RgbColor
    {
        #region Static

        public static bool VerifyPixelParameterRange(RgbColor color, double parameterRangeMinimum, double parameterRangeMaximum, bool throwOnError)
        {
            if (parameterRangeMinimum > color.Red || parameterRangeMaximum < color.Red)
            {
                if (throwOnError)
                {
                    string message = Pixel.FormatExceptionMessage(@"Red", color.Red, parameterRangeMinimum, parameterRangeMaximum);
                    throw new Exception(message);
                }
                else
                {
                    return false;
                }
            }

            if (parameterRangeMinimum > color.Green || parameterRangeMaximum < color.Green)
            {
                if (throwOnError)
                {
                    string message = Pixel.FormatExceptionMessage(@"Green", color.Green, parameterRangeMinimum, parameterRangeMaximum);
                    throw new Exception(message);
                }
                else
                {
                    return false;
                }
            }

            if (parameterRangeMinimum > color.Blue || parameterRangeMaximum < color.Blue)
            {
                if (throwOnError)
                {
                    string message = Pixel.FormatExceptionMessage(@"Blue", color.Blue, parameterRangeMinimum, parameterRangeMaximum);
                    throw new Exception(message);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool VerifyPixelParameterRange(RgbColor color, double parameterRangeMinimum, double parameterRangeMaximum)
        {
            bool output = RgbColor.VerifyPixelParameterRange(color, parameterRangeMinimum, parameterRangeMaximum, false);
            return output;
        }

        public static bool VerifyPixelParameterRange(RgbColor color)
        {
            bool output = RgbColor.VerifyPixelParameterRange(color, Pixel.DefaultColorParameterRangeMinimum, Pixel.DefaultColorParamtereRangeMaximum, false);
            return output;
        }

        public static void VerifyPixelParameterRangeThrow(RgbColor color, double parameterRangeMinimum, double parameterRangeMaximum)
        {
            RgbColor.VerifyPixelParameterRange(color, parameterRangeMinimum, parameterRangeMaximum, true);
        }

        public static void VerifyPixelParameterRangeThrow(RgbColor color)
        {
            RgbColor.VerifyPixelParameterRange(color, Pixel.DefaultColorParameterRangeMinimum, Pixel.DefaultColorParamtereRangeMaximum, true);
        }

        #endregion


        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }


        public RgbColor() { }

        public RgbColor(double red, double green, double blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public RgbColor(Color color)
        {
            this.Red = ColorConversion.LevelToValue(color.R);
            this.Green = ColorConversion.LevelToValue(color.G);
            this.Blue = ColorConversion.LevelToValue(color.B);
        }
    }
}
