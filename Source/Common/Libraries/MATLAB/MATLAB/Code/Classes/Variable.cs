using System;


namespace Public.Common.MATLAB
{
    public class Variable : IDisposable
    {
        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.CleanUp(true);

            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                }

                // Clean-up unmanaged resources here.
                this.MatlabApplication.Clear(this.Name);
            }

            this.zDisposed = true;
        }

        ~Variable()
        {
            this.CleanUp(false);
        }

        #endregion


        private MatlabApplication MatlabApplication { get; }
        public string Name { get; private set; }


        public Variable(MatlabApplication matlabApplication, string name, object value)
        {
            this.MatlabApplication = matlabApplication;
            this.Name = name;

            this.MatlabApplication.PutData(this.Name, value);
        }

        public Variable(MatlabApplication matlabApplication, string name, string creationCommand)
        {
            this.MatlabApplication = matlabApplication;
            this.Name = name;

            this.MatlabApplication.Execute(creationCommand);
        }

        public Variable(MatlabApplication matlabApplication, string name)
            : this(matlabApplication, name, Matlab.GetCreateEmptyStructureCommand(name)) // A structure can always be replaced with a another value, a field cannot be added to a non-structure. Allow addition of fields to crated variables.
        {
        }
    }
}
