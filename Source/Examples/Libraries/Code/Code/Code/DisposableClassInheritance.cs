using System;


namespace Public.Examples.Code
{
    class DisposableClassInheritance : DisposableClass // No need for a redundant IDisposable interface inheritance. The base class already specifies that inheritance.
    {
        #region IDisposable

        // The inherited class needs its own disposed tracker.
        private bool zDisposed = false; // To detect redundant calls.

        // Override the protect virtual dispose method.
        protected override void Dispose(bool disposing)
        {
            // Follow the same pattern as in the base class.
            if(disposing)
            {
                // Free any managed resources here.
            }

            // Free any unmanaged resources here.

            // The derived class is done.
            this.zDisposed = true;

            base.Dispose(disposing);
        }

        // If the derived class has any unmanaged resources, it needs a finalizer.
        //~DisposableClassInheritance()
        //{
        //    Dispose(false);
        //}

        #endregion
    }
}
