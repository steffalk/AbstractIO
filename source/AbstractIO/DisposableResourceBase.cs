using System;

namespace AbstractIO
{
    /// <summary>
    /// A base class implementing the IDisposable interface and pattern and letting the dipose happen in a virtual
    /// method.
    /// </summary>
    public abstract class DisposableResourceBase : IDisposable
    {
        /// <summary>
        /// Disposes the disposable ressource. This method will be called by the Dispose() method when appropriate.
        /// Inheritors should still take care of not disposing the same resource multiple times if that could cause
        /// problems, as this method might be called more than once for the same object.
        /// </summary>
        protected abstract void DisposeResource();

        /// <summary>
        /// Gets whether this object has been disposed.
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                return _disposedValue;
            }
        }

        #region IDisposable Support

        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DisposeResource();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        /// <summary>
        /// The finalizer, ensuring that the unmanaged resources get cleaned up at some (undetermined time) if the
        /// program does not Dispose() this resource correctly and the unused object gets garbage collected.
        /// </summary>
        ~DisposableResourceBase()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
