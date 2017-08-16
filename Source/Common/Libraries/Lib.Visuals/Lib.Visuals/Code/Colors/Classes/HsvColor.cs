//using System;
//using System.Drawing;


//namespace Public.Common.Lib.Visuals
//{
//    /// <summary>
//    /// Represents a color specified by HSV values. All HSV values should be in the range [0, 1].
//    /// </summary>
//    [Serializable]
//    public class HsvColor
//    {
//        #region Static

//        public static bool VerifyPixelParameterRange(HsvColor color, double parameterRangeMinimum, double parameterRangeMaximum, bool throwOnError)
//        {
//            if (parameterRangeMinimum > color.Hue || parameterRangeMaximum < color.Hue)
//            {
//                if (throwOnError)
//                {
//                    string message = Pixel.FormatExceptionMessage(@"Hue", color.Hue, parameterRangeMinimum, parameterRangeMaximum);
//                    throw new Exception(message);
//                }
//                else
//                {
//                    return false;
//                }
//            }

//            if (parameterRangeMinimum > color.Saturation || parameterRangeMaximum < color.Saturation)
//            {
//                if (throwOnError)
//                {
//                    string message = Pixel.FormatExceptionMessage(@"Saturation", color.Saturation, parameterRangeMinimum, parameterRangeMaximum);
//                    throw new Exception(message);
//                }
//                else
//                {
//                    return false;
//                }
//            }

//            if (parameterRangeMinimum > color.Value || parameterRangeMaximum < color.Value)
//            {
//                if (throwOnError)
//                {
//                    string message = Pixel.FormatExceptionMessage(@"Value", color.Value, parameterRangeMinimum, parameterRangeMaximum);
//                    throw new Exception(message);
//                }
//                else
//                {
//                    return false;
//                }
//            }

//            return true;
//        }

//        public static bool VerifyPixelParameterRange(HsvColor color, double parameterRangeMinimum, double parameterRangeMaximum)
//        {
//            bool output = HsvColor.VerifyPixelParameterRange(color, parameterRangeMinimum, parameterRangeMaximum, false);
//            return output;
//        }

//        public static bool VerifyPixelParameterRange(HsvColor color)
//        {
//            bool output = HsvColor.VerifyPixelParameterRange(color, Pixel.DefaultColorParameterRangeMinimum, Pixel.DefaultColorParameterRangeMaximum, false);
//            return output;
//        }

//        public static void VerifyPixelParameterRangeThrow(HsvColor color, double parameterRangeMinimum, double parameterRangeMaximum)
//        {
//            HsvColor.VerifyPixelParameterRange(color, parameterRangeMinimum, parameterRangeMaximum, true);
//        }

//        public static void VerifyPixelParameterRangeThrow(HsvColor color)
//        {
//            HsvColor.VerifyPixelParameterRange(color, Pixel.DefaultColorParameterRangeMinimum, Pixel.DefaultColorParameterRangeMaximum, true);
//        }

//        /// <remarks>
//        /// A second constructor allowing three doubles.
//        /// </remarks>
//        public static HsvColor FromRGB(double red, double green, double blue)
//        {
//            double hue; double saturation; double value;
//            ColorConversion.RgbToHsv(red, green, blue, out hue, out saturation, out value);

//            HsvColor output = new HsvColor(hue, saturation, value);
//            return output;
//        }

//        #endregion

//        public double Hue { get; set; }
//        public double Saturation { get; set; }
//        public double Value { get; set; }


//        public HsvColor() { }

//        public HsvColor(double hue, double saturation, double value)
//        {
//            this.Hue = hue;
//            this.Saturation = saturation;
//            this.Value = value;
//        }

//        public HsvColor(RgbColor rgbColor)
//        {
//            double hue; double saturation; double value;
//            ColorConversion.RgbToHsv(rgbColor, out hue, out saturation, out value);

//            this.Hue = hue;
//            this.Saturation = saturation;
//            this.Value = value;
//        }

//        public HsvColor(Color color)
//        {
//            RgbColor rgbColor;
//            ColorConversion.ColorToRgb(color, out rgbColor);

//            double hue; double saturation; double value;
//            ColorConversion.RgbToHsv(rgbColor, out hue, out saturation, out value);

//            this.Hue = hue;
//            this.Saturation = saturation;
//            this.Value = value;
//        }
//    }
//}
