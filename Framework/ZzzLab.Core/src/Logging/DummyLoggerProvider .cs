using Microsoft.Extensions.Logging;
using System;

namespace ZzzLab.Logging
{
    //[UnsupportedOSPlatform("browser")]
    [ProviderAlias("ZLogger")]
    public sealed class ZLoggerProvider<TClass> : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            if (Activator.CreateInstance(typeof(TClass)) is ILogger logger) return logger;
            throw new Exception("ZLoggerProvider fail");
        }

        #region IDisposable

        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}