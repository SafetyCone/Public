using System;


namespace Examples
{
    /// <summary>
    /// Adapted from the Visual Studio IDisposable implementation code-snippet.
    /// </summary>
    class DisposableClass : IDisposable
    {
        #region IDisposable Members

        private bool zDisposed = false; // To detect redundant calls.


        public void Dispose()
        {
            this.Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden.
            //GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
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
