using System;


namespace Public.Common.Lib.Code.Physical
{
    public enum ErrorReport
    {
        Prompt,
    }


    public static class ErrorReportExtensions
    {
        public const string Prompt = @"prompt";


        public static string ToDefaultString(this ErrorReport errorReport)
        {
            string output;
            switch (errorReport)
            {
                case ErrorReport.Prompt:
                    output = ErrorReportExtensions.Prompt;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ErrorReport>(errorReport);
            }

            return output;
        }

        public static ErrorReport FromDefault(string errorReport)
        {
            ErrorReport output;
            if (!ErrorReportExtensions.TryFromDefault(errorReport, out output))
            {
                string message = String.Format(@"Unrecognized error report string: {0}.", errorReport);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string errorReport, out ErrorReport value)
        {
            bool output = true;
            value = ErrorReport.Prompt;

            switch (errorReport)
            {
                case ErrorReportExtensions.Prompt:
                    value = ErrorReport.Prompt;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
