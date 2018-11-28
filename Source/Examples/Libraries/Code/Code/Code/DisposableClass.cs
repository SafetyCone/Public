using System;


namespace Public.Examples.Code
{
    /// <summary>
    /// Adapted from the Visual Studio IDisposable implementation code-snippet.
    /// Also: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
    /// </summary>
    // sealed - Consider making the class sealed to remove the virtual qualifier from the protected dispose call below.
    class DisposableClass : IDisposable
    {
        #region IDisposable

        private bool zDisposed = false; // To detect redundant calls.


        public void Dispose()
        {
            this.Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden.
            //GC.SuppressFinalize(this);
        }

        // Remove the virtual call if the class is sealed (or has no plans for subclassing, in which case this should be communicated by sealing the class).
        protected virtual void Dispose(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // TODO: Call Dispose() on any contained managed disposable resources here.
                }

                // TODO: Clean-up unmanaged resources here by freeing unmanaged objects and override the finalizer below.
                // TODO: Set any large fields to null.
            }

            this.zDisposed = true;
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        //~DisposableClass()
        //{
        //    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    this.Dispose(false);
        //}

        #endregion
    }
}
