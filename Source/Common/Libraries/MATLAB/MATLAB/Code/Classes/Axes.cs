using System;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Represents a MATLAB axes on a Figure.
    /// </summary>
    public class Axes : HandleBase
    {
        private const string AxesHandleTypePrefix = @"hAxes";
        private const string CreateAxesFunctionName = @"axes";
        private const string XLimitPropertyName = @"XLim";
        private const string YLimitPropertyName = @"YLim";
        private const string ZLimitPropertyName = @"ZLim";


        #region Static

        public static double[] GetTicks(double low, double high, double step)
        {
            int numElements = Convert.ToInt32((high - low) / step) + 1;

            double[] output = new double[numElements];
            for (int iElement = 0; iElement < numElements; iElement++)
            {
                output[iElement] = low + iElement * step;
            }

            return output;
        }

        #endregion


        public Figure Figure { get; protected set; }
        protected override string HandleTypePrefix
        {
            get
            {
                return Axes.AxesHandleTypePrefix;
            }
        }
        public double XLimitMax
        {
            get
            {
                double[,] xLim = (double[,])this.GetProperty(Axes.XLimitPropertyName);
                return xLim[0, 1];
            }
            set
            {
                string propertyValueAsString = String.Format(@"[{0}, {1}]", this.XLimitMin, value);
                this.SetProperty(Axes.XLimitPropertyName, propertyValueAsString);
            }
        }
        public double XLimitMin
        {
            get
            {
                double[,] xLim = (double[,])this.GetProperty(Axes.XLimitPropertyName);

                double output = xLim[0, 0];
                return output;
            }
            set
            {
                string propertyValueAsString = String.Format(@"[{0}, {1}]", value, this.XLimitMax);
                this.SetProperty(Axes.XLimitPropertyName, propertyValueAsString);
            }
        }
        public double YLimitMax
        {
            get
            {
                double[,] yLim = (double[,])this.GetProperty(Axes.YLimitPropertyName);
                return yLim[0, 1];
            }
            set
            {
                string propertyValueAsString = String.Format(@"[{0}, {1}]", this.YLimitMin, value);
                this.SetProperty(Axes.YLimitPropertyName, propertyValueAsString);
            }
        }
        public double YLimitMin
        {
            get
            {
                double[,] yLim = (double[,])this.GetProperty(Axes.YLimitPropertyName);
                return yLim[0, 0];
            }
            set
            {
                string propertyValueAsString = String.Format(@"[{0}, {1}]", value, this.YLimitMax);
                this.SetProperty(Axes.YLimitPropertyName, propertyValueAsString);
            }
        }

        public double ZLimitMax
        {
            get
            {
                double[,] zLim = (double[,])this.GetProperty(Axes.ZLimitPropertyName);
                return zLim[0, 1];
            }
            set
            {
                string propertyValueAsString = String.Format(@"[{0}, {1}]", this.ZLimitMin, value);
                this.SetProperty(Axes.ZLimitPropertyName, propertyValueAsString);
            }
        }
        public double ZLimitMin
        {
            get
            {
                double[,] zLim = (double[,])this.GetProperty(Axes.ZLimitPropertyName);
                return zLim[0, 0];
            }
            set
            {
                string propertyValueAsString = String.Format(@"[{0}, {1}]", value, this.ZLimitMax);
                this.SetProperty(Axes.ZLimitPropertyName, propertyValueAsString);
            }
        }


        public Axes(Figure figure)
            : base(figure.Application)
        {
            this.Figure = figure;

            string command = String.Format(@"{0} = {1}('Parent', {2})", this.HandleName, Axes.CreateAxesFunctionName, this.Figure.HandleName);
            this.Application.Execute(command);
        }

        public Axes()
            : this(new Figure())
        {
        }

        public void SetXLimits(double min, double max)
        {
            string propertyValueAsString = String.Format(@"[{0}, {1}]", min, max);
            this.SetProperty(Axes.XLimitPropertyName, propertyValueAsString);
        }

        public void SetYLimits(double min, double max)
        {
            string propertyValueAsString = String.Format(@"[{0}, {1}]", min, max);
            this.SetProperty(Axes.YLimitPropertyName, propertyValueAsString);
        }

        public void SetZLimits(double min, double max)
        {
            string propertyValueAsString = String.Format(@"[{0}, {1}]", min, max);
            this.SetProperty(Axes.ZLimitPropertyName, propertyValueAsString);
        }

        public void SetLimits(double xMin, double xMax, double yMin, double yMax, double zMin, double zMax)
        {
            this.SetXLimits(xMin, xMax);
            this.SetYLimits(yMin, yMax);
            this.SetZLimits(zMin, zMax);
        }

        /// <summary>
        /// Sets the axis limits in a cube, using the same min and max for all three axes.
        /// </summary>
        public void SetLimits(double min, double max)
        {
            this.SetXLimits(min, max);
            this.SetYLimits(min, max);
            this.SetZLimits(min, max);
        }

        public void SetXLabel(string label)
        {
            string command = String.Format(@"xlabel({0}, '{1}')", this.HandleName, label);
            this.Application.Execute(command);
        }

        public void SetXLabel()
        {
            this.SetXLabel(@"X");
        }

        public void SetYLabel(string label)
        {
            string command = String.Format(@"ylabel({0}, '{1}')", this.HandleName, label);
            this.Application.Execute(command);
        }

        public void SetYLabel()
        {
            this.SetYLabel(@"Y");
        }

        public void SetZLabel(string label)
        {
            string command = String.Format(@"zlabel({0}, '{1}')", this.HandleName, label);
            this.Application.Execute(command);
        }

        public void SetZLabel()
        {
            this.SetZLabel(@"Z");
        }

        public void SetAxisLabels(string xLabel, string yLabel, string zLabel)
        {
            this.SetXLabel(xLabel);
            this.SetYLabel(yLabel);
            this.SetZLabel(zLabel);
        }

        public void SetAxisLabels()
        {
            this.SetXLabel();
            this.SetYLabel();
            this.SetZLabel();
        }

        public void SetXTicks(double[] ticks)
        {
            string variableName = this.HandleName + @"xTicks";
            this.Application.PutData(variableName, ticks);

            string command;
            command = String.Format(@"set({0}, 'XTick', {1})", this.HandleName, variableName);
            this.Application.Execute(command);

            command = String.Format(@"clear {0}", variableName);
            this.Application.Execute(command);
        }

        public void SetXTicks(double low, double high, double step)
        {
            double[] ticks = Axes.GetTicks(low, high, step);
            this.SetXTicks(ticks);
        }

        public void SetYTicks(double[] ticks)
        {
            string variableName = this.HandleName + @"yTicks";
            this.Application.PutData(variableName, ticks);

            string command;
            command = String.Format(@"set({0}, 'YTick', {1})", this.HandleName, variableName);
            this.Application.Execute(command);

            command = String.Format(@"clear {0}", variableName);
            this.Application.Execute(command);
        }

        public void SetYTicks(double low, double high, double step)
        {
            double[] ticks = Axes.GetTicks(low, high, step);
            this.SetXTicks(ticks);
        }

        public void SetZTicks(double[] ticks)
        {
            string variableName = this.HandleName + @"zTicks";
            this.Application.PutData(variableName, ticks);

            string command;
            command = String.Format(@"set({0}, 'ZTick', {1})", this.HandleName, variableName);
            this.Application.Execute(command);

            command = String.Format(@"clear {0}", variableName);
            this.Application.Execute(command);
        }

        public void SetZTicks(double low, double high, double step)
        {
            double[] ticks = Axes.GetTicks(low, high, step);
            this.SetXTicks(ticks);
        }

        public void SetTicks(double[] xTicks, double[] yTicks, double[] zTicks)
        {
            this.SetXTicks(xTicks);
            this.SetYTicks(yTicks);
            this.SetZTicks(zTicks);
        }

        public void SetGrid(bool onOff)
        {
            string command;
            if(onOff)
            {
                command = @"grid on";
            }
            else
            {
                command = @"grid off";
            }

            this.Application.Execute(command);
        }

        public void SetViewXY()
        {
            string command = @"view(0, 90)";
            this.Application.Execute(command);
        }

        public void SetViewXZ()
        {
            string command = @"view(0, 0)";
            this.Application.Execute(command);
        }

        public void SetViewYZ()
        {
            string command = @"view(90, 0)";
            this.Application.Execute(command);
        }
    }
}

