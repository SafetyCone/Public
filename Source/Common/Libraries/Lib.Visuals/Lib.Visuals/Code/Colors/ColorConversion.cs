using System;
using System.Drawing;


namespace Minex.Common.Lib.Visuals
{
    public class ColorConversion
    {
        public const double ByteColorMaxValue = 255;
        // Component parameters taken from MATLAB's rgb2gray function.
        public const double GrayRedComponent = 0.2989;
        public const double GrayGreenComponent = 0.5870;
        public const double GrayBlueComponent = 0.1140;


        #region Static

        public static void ColorToRgb(Color color, out RgbColor rgbColor)
        {
            double red = ColorConversion.LevelToValue(color.R);
            double green = ColorConversion.LevelToValue(color.G);
            double blue = ColorConversion.LevelToValue(color.B);

            rgbColor = new RgbColor(red, green, blue);
        }

        /// <summary>
        /// Calculates the HSV color values for a set of RGB color values. Input values must be on the range [0, 1]. Output values are in the range [0, 1].
        /// </summary>
        /// <remarks>
        /// Adapted from: http://www.rapidtables.com/convert/color/rgb-to-hsv.htm.
        /// </remarks>
        public static void RgbToHsv(double red, double green, double blue, out double hue, out double saturation, out double value)
        {
            double colorMax = Math.Max(Math.Max(red, green), blue);
            double colorMin = Math.Min(Math.Min(red, green), blue);

            double delta = colorMax - colorMin;

            // Value is easy.
            value = colorMax;

            // Saturation is medium.
            if (0 == colorMax)
            {
                saturation = 0;
            }
            else
            {
                saturation = delta / colorMax;
            }

            // Hue is hard.
            double unNormalizedHue;
            if (0 == delta)
            {
                unNormalizedHue = 0;
            }
            else
            {
                if (red == colorMax)
                {
                    unNormalizedHue = (green - blue) / delta;
                    if (0 > unNormalizedHue)
                    {
                        unNormalizedHue += 6;
                    }
                }
                else
                {
                    if (green == colorMax)
                    {
                        unNormalizedHue = (blue - red) / delta + 2;
                    }
                    else
                    {
                        // Blue is the max color.
                        unNormalizedHue = (blue - red) / delta + 4;
                    }
                }
            }

            hue = unNormalizedHue / 6;
        }

        public static void RgbToHsv(RgbColor rgbColor, out double hue, out double saturation, out double value)
        {
            ColorConversion.RgbToHsv(rgbColor.Red, rgbColor.Green, rgbColor.Blue, out hue, out saturation, out value);
        }

        public static void RgbToHsv(RgbColor rgbColor, out HsvColor hsvColor)
        {
            hsvColor = new HsvColor(rgbColor);
        }

        /// <summary>
        /// Calculates the RGB color values for a set of HSV color values. Input values must be on the range [0, 1]. Output values are in the range [0, 1].
        /// </summary>
        /// <remarks>
        /// Adapted from: http://www.rapidtables.com/convert/color/hsv-to-rgb.htm.
        /// </remarks>
        public static void HsvToRgb(double hue, double saturation, double value, out double red, out double green, out double blue)
        {
            double hexaHue = hue * 6;

            double delta = value * saturation;

            double redPrime = 0;
            double greenPrime = 0;
            double bluePrime = 0;

            double x = 0;
            if (hexaHue < 2)
            {
                x = delta * (1 - Math.Abs(hexaHue - 1));

                if (hexaHue < 1)
                {
                    redPrime = delta;
                    greenPrime = x;
                }
                else
                {
                    redPrime = x;
                    greenPrime = delta;
                }
            }
            else
            {
                if (hexaHue < 4)
                {
                    x = delta * (1 - Math.Abs(hexaHue - 3));

                    if (hexaHue < 3)
                    {
                        greenPrime = delta;
                        bluePrime = x;
                    }
                    else
                    {
                        greenPrime = x;
                        bluePrime = delta;
                    }
                }
                else
                {
                    x = delta * (1 - Math.Abs(hexaHue - 5));

                    if (hexaHue < 5)
                    {
                        redPrime = x;
                        bluePrime = delta;
                    }
                    else
                    {
                        redPrime = delta;
                        bluePrime = x;
                    }
                }
            }

            double m = value - delta;

            red = redPrime + m;
            green = greenPrime + m;
            blue = bluePrime + m;
        }

        public static void HsvToRgb(HsvColor hsvColor, out double red, out double green, out double blue)
        {
            ColorConversion.HsvToRgb(hsvColor.Hue, hsvColor.Saturation, hsvColor.Value, out red, out green, out blue);
        }

        public static void HsvToRgb(HsvColor hsvColor, out RgbColor rgbColor)
        {
            double red; double green; double blue;
            ColorConversion.HsvToRgb(hsvColor, out red, out green, out blue);

            rgbColor = new RgbColor(red, green, blue);
        }

        /// <summary>
        /// Calculates a gray value for a set of RGB color values. Input values must be on the range [0, 1]. Output value is in the range [0, 1].
        /// </summary>
        /// <remarks>
        /// Coefficients taken from MATLAB's rgb2gray function.
        /// </remarks>
        public static double RgbToGray(double red, double green, double blue)
        {
            double output = ColorConversion.GrayRedComponent * red + ColorConversion.GrayGreenComponent * green + ColorConversion.GrayBlueComponent * blue;
            return output;
        }

        public static double RgbToGray(RgbColor rgbColor)
        {
            double output = ColorConversion.RgbToGray(rgbColor.Red, rgbColor.Green, rgbColor.Blue);
            return output;
        }

        /// <summary>
        /// Calculates a gray value for a set of RGB color values. Output value is in the range [0, 1].
        /// </summary>
        /// <remarks>
        /// Coefficients taken from MATLAB's rgb2gray function.
        /// </remarks>
        public static double RgbToGray(byte red, byte green, byte blue)
        {
            double output = ColorConversion.RgbToGray((double)red, (double)green, (double)blue);
            return output;
        }

        /// <summary>
        /// Calculates the level for a value in the range [0, 1] given a maximum value of the level.
        /// </summary>
        /// <param name="maxLevel">
        /// The maximum level parameter is a double to suggest that the conversion of the maximum level (usually an integer) to a double should be done once, instead of every iteration loop.
        /// </param>
        /// <remarks>
        /// Color values by default should be in the range [0, 1], but when reading or writing colors externally, generally bytes in the range [0, 255] are used.
        /// This function allows easy conversion from color values to color levels.
        /// </remarks>
        public static int ValueToLevel(double value, double maxLevel)
        {
            int output = (int)Math.Round(value * maxLevel);
            return output;
        }

        /// <summary>
        /// Calculates the level for a value in the range [0, 1] assuming the level is on the range [0, 255];
        /// </summary>
        public static int ValueToLevel(double value)
        {
            int output = ColorConversion.ValueToLevel(value, ColorConversion.ByteColorMaxValue);
            return output;
        }

        /// <summary>
        /// Calculates the level for a value in the range [0, 1] given a maximum value of the level.
        /// </summary>
        /// <remarks>
        /// Color values by default should be in the range [0, 1], but when reading or writing colors externally, generally bytes in the range [0, 255] are used.
        /// This function allows easy conversion from color values to color levels.
        /// </remarks>
        public static byte ValueToLevelByte(double value, double maxLevel)
        {
            byte output = (byte)Math.Round(value * maxLevel);
            return output;
        }

        /// <summary>
        /// Calculates the level for a value in the range [0, 1] assuming the level is on the range [0, 255];
        /// </summary>
        public static byte ValueToLevelByte(double value)
        {
            byte output = ColorConversion.ValueToLevelByte(value, ColorConversion.ByteColorMaxValue);
            return output;
        }

        /// <summary>
        /// Calculates the color value for a level given the maximum possible value for the level.
        /// </summary>
        /// <remarks>
        /// The color value is on the range [0, 1], while the level is generally on the range [0, 255]. But the max level can be specified.
        /// </remarks>
        public static double LevelToValue(int level, double maxLevel)
        {
            double output = (double)level / maxLevel;
            return output;
        }

        /// <summary>
        /// Calculates the color value for a level assuming the level is on the range [0, 255].
        /// </summary>
        public static double LevelToValue(int level)
        {
            double output = ColorConversion.LevelToValue(level, ColorConversion.ByteColorMaxValue);
            return output;
        }

        /// <summary>
        /// Calculates the color value for a level given the maximum possible value for the level.
        /// </summary>
        /// <remarks>
        /// The color value is a double on the range [0, 1], while the level is generally a byte on the range [0, 255].
        /// </remarks>
        public static double LevelToValue(byte level)
        {
            double output = (double)level / ColorConversion.ByteColorMaxValue;
            return output;
        }

        #endregion
    }
}
