using System;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Represents a MATLAB figure.
    /// </summary>
    public class Figure : HandleBase
    {
        private const string FigureHandleTypePrefix = @"hFig";
        private const string CreateFigureFunctionName = @"figure";


        protected override string HandleTypePrefix
        {
            get
            {
                return Figure.FigureHandleTypePrefix;
            }
        }
        private bool zHold;
        public bool Hold
        {
            get
            {
                return zHold;
            }
            set
            {
                this.zHold = value;

                if (this.zHold)
                {
                    this.Application.Execute(@"hold on");
                }
                else
                {
                    this.Application.Execute(@"hold off");
                }
            }
        }


        public Figure(MatlabApplication matlabApplication)
            : base(matlabApplication)
        {
            string command = String.Format(@"{0} = {1}", this.HandleName, Figure.CreateFigureFunctionName);
            this.Application.Execute(command);
        }

        public Figure()
            : this(MatlabApplication.Instance)
        {
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Save(string filePath)
        {
            string command = String.Format(@"saveas({0}, '{1}')", this.HandleName, filePath);
            this.Application.Execute(command);
        }
    }
}
