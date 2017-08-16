//using System;


//namespace Public.Common.Lib.Visuals
//{
//    [Serializable]
//    public abstract class PixelBase : ICoordinated
//    {
//        public const double DefaultColorParameterRangeMinimum = 0;
//        public const double DefaultColorParameterRangeMaximum = 1;


//        #region Static

//        public static string FormatExceptionMessage(string parameterName, double value, double parameterRangeMinimum, double parameterRangeMaximum)
//        {
//            string output = String.Format(@"Color parameter '{0}' outside allowed range. Value: {1}, allowed range: [{2} {3}].", parameterName, value, parameterRangeMinimum, parameterRangeMaximum);
//            return output;
//        }

//        public static string FormatExceptionMessage(string parameterName, double value)
//        {
//            string output = PixelBase.FormatExceptionMessage(parameterName, value, PixelBase.DefaultColorParameterRangeMinimum, PixelBase.DefaultColorParameterRangeMaximum);
//            return output;
//        }

//        #endregion

//        #region ICoordinated Members

//        public Coordinate Coordinate { get; set; }

//        #endregion


//        public PixelBase() { }

//        public PixelBase(int row, int column) : this(new Coordinate(row, column)) { }

//        public PixelBase(Coordinate coordinate)
//        {
//            this.Coordinate = coordinate;
//        }
//    }
//}
