using System;
using System.Collections.Generic;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Base class for handles, implementing disposable and handle naming functionality.
    /// </summary>
    public abstract class HandleBase : IHandle
    {
        #region Static

        private static Dictionary<string, int> zNextHandleNumberByHandleTypePrefix;


        static HandleBase()
        {
            HandleBase.zNextHandleNumberByHandleTypePrefix = new Dictionary<string, int>();
        }

        protected static string GetNextHandleName(string handleTypePrefix)
        {
            if(!HandleBase.zNextHandleNumberByHandleTypePrefix.ContainsKey(handleTypePrefix))
            {
                HandleBase.zNextHandleNumberByHandleTypePrefix.Add(handleTypePrefix, 0);
            }

            int nextHandleNumber = HandleBase.zNextHandleNumberByHandleTypePrefix[handleTypePrefix]++;

            string output = handleTypePrefix + nextHandleNumber.ToString();
            return output;
        }

        #endregion

        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.CleanUp(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void CleanUp(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                }

                // Clean-up unmanaged resources here.
                string command = String.Format(@"{0}({1})", Matlab.DeleteFunctionName, this.HandleName);
                this.Application.Execute(command);
            }

            this.zDisposed = true;
        }

        ~HandleBase()
        {
            this.CleanUp(false);
        }

        #endregion


        public MatlabApplication Application { get; protected set; }
        public string HandleName { get; protected set; }
        protected abstract string HandleTypePrefix { get; }


        protected HandleBase(MatlabApplication application)
        {
            this.Application = application;
            this.HandleName = HandleBase.GetNextHandleName(this.HandleTypePrefix);
        }

        /// <summary>
        /// Uses the MATLAB application instance.
        /// </summary>
        protected HandleBase()
            : this(MatlabApplication.Instance)
        {
        }

        /// <summary>
        /// Delete the handle object in MATLAB.
        /// </summary>
        /// <remarks>
        /// Same as Dispose().
        /// </remarks>
        public void Delete()
        {
            this.Dispose();
        }

        protected object GetProperty(string propertyName)
        {
            string tempPropertyVariableName = this.HandleName + @"temp"; // Use a temp variable to hold the answer and bring it into .NET.

            string command;
            command = String.Format(@"{0} = get({1}, '{2}')", tempPropertyVariableName, this.HandleName, propertyName);
            this.Application.Execute(command);

            object output = this.Application.GetData(tempPropertyVariableName);

            command = String.Format(@"clear {0}", tempPropertyVariableName); // Clean up the variable name.
            this.Application.Execute(command);

            return output;
        }

        protected void SetProperty(string propertyName, string propertyValueAsString)
        {
            string command = String.Format(@"set({0}, '{1}', {2})", this.HandleName, propertyName, propertyValueAsString);
            string result = this.Application.Execute(command);
        }
    }
}
