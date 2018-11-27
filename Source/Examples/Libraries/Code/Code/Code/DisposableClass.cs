using System;


namespace Examples
{
    class DisposableClass : IDisposable
    {
        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~DisposableClass()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
