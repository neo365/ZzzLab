using ZzzLab.Configuration;
using ZzzLab.Web.Auth;

namespace ZzzLab.Web.Configuration
{
    internal class WebBuilder : IServiceBuilder, IDisposable
    {
        internal static IAuthorization? AuthConfig { set; get; }

        internal WebBuilder()
        {
        }

        internal WebBuilder(IAuthorization? auth = null) : this()
        {
            if (auth != null) AuthConfig = auth;
        }

        #region IServiceBuilder

        public void Initialize(IConfigBuilder configBuilder, params object[] args)
        {
        }

        public void Load(IConfigBuilder configBuilder, params object[] args)
        {
        }

        public void ReLoad(IConfigBuilder configBuilder, params object[] args)
        {
        }

        #endregion IServiceBuilder

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}