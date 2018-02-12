using System.Text;


namespace Public.Common.MATLAB.Extensions
{
    public static class ArrayIntExtensions
    {
        public static string ToSizeString(this int[] size)
        {
            int nDimensions = size.Length;

            string output;
            if(1 > nDimensions)
            {
                output = @"<Empty>";
            }
            else
            {
                StringBuilder builder = new StringBuilder(size[0].ToString());
                for (int iDimension = 1; iDimension < nDimensions; iDimension++)
                {
                    string appendix = $@"x{size[iDimension].ToString()}";
                    builder.Append(appendix);
                }

                string builtString = builder.ToString();
                output = $@"[{builtString}]";
            }

            return output;
        }
    }
}
