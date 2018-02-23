using System;


namespace Eshunna.Lib
{
    public static class Extensions
    {
        /// <summary>
        /// Formats a double to have exactly 12 digits, i.e. 103.726450237 or 10.3726450237 or 1.03726450237 or -0.0700699378582 or -0.00945108265552;
        /// </summary>
        /// <remarks>
        /// Ignored differences:
        /// * 4.4408920985e-016 (expected) 4.44089209850e-016 (actual).
        /// </remarks>
        public static string FormatNvm12SignificantDigits(this double value)
        {
            string output;
            if (0 == value)
            {
                output = @"0";
            }
            else
            {
                string negativity = value < 0 ? @"-" : String.Empty;

                double absValue = Math.Abs(value);

                int integerDigits = Convert.ToInt32(Math.Floor(Math.Log10(absValue))) + 1;

                if (-10 < integerDigits)
                {
                    long twelveDigits = Convert.ToInt64(Math.Round(absValue * Math.Pow(10, 12 - integerDigits)));

                    string twelveDigitsStr = String.Format(@"{0:############}", twelveDigits);

                    string integer;
                    string fractionalPrefix;
                    string fractional;
                    if (integerDigits < 1)
                    {
                        integer = @"0";
                        fractionalPrefix = Extensions.GetStringNTimes(@"0", -1 * integerDigits);
                        fractional = twelveDigitsStr;
                    }
                    else
                    {
                        integer = twelveDigitsStr.Substring(0, integerDigits);
                        fractionalPrefix = String.Empty;
                        fractional = twelveDigitsStr.Substring(integerDigits);
                    }

                    string candidate = $@"{negativity}{integer}.{fractionalPrefix}{fractional}";

                    string noTrailingZeros = candidate.TrimEnd('0');
                    output = noTrailingZeros.TrimEnd('.'); // If all zeros have been removed, then also remove the decimal point.
                }
                else
                {
                    // Small number, use scientific notation.
                    output = String.Format(@"{0:e11}", value);
                }
            }
            return output;
        }

        /// <summary>
        /// Formats a double to have exactly 6 digits.
        /// </summary>
        /// <remarks>
        /// Ignored differences:
        /// * -6.1959e-005 (expected) -6.19590e-005 (actual).
        /// </remarks>
        public static string FormatPatch6SignificantDigits(this double value)
        {
            string output;
            if (0 == value)
            {
                output = @"0";
            }
            else
            {
                string negativity = value < 0 ? @"-" : String.Empty;

                double absValue = Math.Abs(value);

                int integerDigits = Convert.ToInt32(Math.Floor(Math.Log10(absValue))) + 1;

                if (-4 < integerDigits)
                {
                    long twelveDigits = Convert.ToInt64(Math.Round(absValue * Math.Pow(10, 6 - integerDigits)));

                    string twelveDigitsStr = String.Format(@"{0:######}", twelveDigits);

                    string integer;
                    string fractionalPrefix;
                    string fractional;
                    if (integerDigits < 1)
                    {
                        integer = @"0";
                        fractionalPrefix = Extensions.GetStringNTimes(@"0", -1 * integerDigits);
                        fractional = twelveDigitsStr;
                    }
                    else
                    {
                        integer = twelveDigitsStr.Substring(0, integerDigits);
                        fractionalPrefix = String.Empty;
                        fractional = twelveDigitsStr.Substring(integerDigits);
                    }

                    string candidate = $@"{negativity}{integer}.{fractionalPrefix}{fractional}";

                    string noTrailingZeros = candidate.TrimEnd('0');
                    output = noTrailingZeros.TrimEnd('.'); // If all zeros have been removed, then also remove the decimal point.
                }
                else
                {
                    // Small number, use scientific notation.
                    output = String.Format(@"{0:e5}", value);
                }
            }
            return output;
        }

        private static string GetStringNTimes(string str, int n)
        {
            string output = String.Empty;
            for (int i = 0; i < n; i++)
            {
                output += str;
            }
            return output;
        }
    }
}
