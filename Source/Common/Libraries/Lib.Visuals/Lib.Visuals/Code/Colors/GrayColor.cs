//using System;


//namespace Public.Common.Lib.Visuals
//{
//    [Serializable]
//    public class GrayColor
//    {
//        #region Static

//        public static bool VerifyPixelParameterRange(GrayColor color, double parameterRangeMinimum, double parameterRangeMaximum, bool throwOnError)
//        {
//            if (parameterRangeMinimum > color.Gray || parameterRangeMaximum < color.Gray)
//            {
//                if (throwOnError)
//                {
//                    string message = Pixel.FormatExceptionMessage(@"Gray", color.Gray, parameterRangeMinimum, parameterRangeMaximum);
//                    throw new Exception(message);
//                }
//                else
//                {
//                    return false;
//                }
//            }

//            return true;
//        }

//        public static bool VerifyPixelParameterRange(GrayColor color, double parameterRangeMinimum, double parameterRangeMaximum)
//        {
//            bool output = GrayColor.VerifyPixelParameterRange(color, parameterRangeMinimum, parameterRangeMaximum, false);
//            return output;
//        }

//        public static bool VerifyPixelParameterRange(GrayColor color)
//        {
//            bool output = GrayColor.VerifyPixelParameterRange(color, Pixel.DefaultColorParameterRangeMinimum, Pixel.DefaultColorParameterRangeMaximum, false);
//            return output;
//        }

//        public static void VerifyPixelParameterRangeThrow(GrayColor color, double parameterRangeMinimum, double parameterRangeMaximum)
//        {
//            GrayColor.VerifyPixelParameterRange(color, parameterRangeMinimum, parameterRangeMaximum, true);
//        }

//        public static void VerifyPixelParameterRangeThrow(GrayColor color)
//        {
//            GrayColor.VerifyPixelParameterRange(color, Pixel.DefaultColorParameterRangeMinimum, Pixel.DefaultColorParameterRangeMaximum, true);
//        }

//        #endregion


//        public double Gray { get; set; }


//        public GrayColor() { }

//        public GrayColor(RgbColor rgbColor)
//        {
//            this.Gray = ColorConversion.RgbToGray(rgbColor);
//        }
//    }
//}
