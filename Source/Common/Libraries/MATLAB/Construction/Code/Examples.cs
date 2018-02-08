

namespace Public.Common.MATLAB.Construction
{
    public static class Examples
    {
        public static void SubMain()
        {
            Examples.FigureXYPlotGetImageData();
        }

        public static void FigureXYPlotGetImageData()
        {
            using (MatlabApplication matlabApplication = new MatlabApplication())
            {
                matlabApplication.Execute(@"x = 0:0.1:2*pi;");
                matlabApplication.Execute(@"y = sin(x);");
                double[] x = matlabApplication.GetRealVector(@"x");
                double[] y = matlabApplication.GetRealVector(@"y");

                using (Figure figure = new Figure(matlabApplication))
                {
                    matlabApplication.Execute(@"plot(x, y)");
                    matlabApplication.Execute($@"f = getframe({figure.HandleName})");
                    matlabApplication.Execute(@"cdata = f.cdata;");
                    object temp = matlabApplication.GetData(@"cdata");
                }
            }
        }
    }
}
