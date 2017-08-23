using System;


namespace Public.Common.MATLAB
{
    public class MatlabException : ApplicationException
    {
        public MatlabException()
            : base()
        {
        }

        public MatlabException(string message)
            : base(message)
        {
        }

        public MatlabException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
