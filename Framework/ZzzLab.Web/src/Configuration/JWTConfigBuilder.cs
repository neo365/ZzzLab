using ZzzLab.Configuration;

namespace ZzzLab.Web.Configuration
{
    internal class JWTConfigBuilder : IServiceBuilder, IDisposable
    {
        internal static IJWTConfigurationLoader? BaseReader { get; set; }

        internal JWTConfigBuilder(IJWTConfigurationLoader reader)
        {
            BaseReader = reader;
        }

        public void Initialize(IConfigBuilder configBuilder, params object[] args)
        {
        }

        public void Load(IConfigBuilder configBuilder, params object[] args)
        {
            try
            {
                Configurator.Setting.JWTConfig = new JWTConfig();

                if (BaseReader == null) return;

                var configs = BaseReader.Reader();

                if (configs != null && configs.Any())
                {
                    Configurator.Setting.JWTConfig = configs.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }
        }

        public void ReLoad(IConfigBuilder configBuilder, params object[] args)
            => Load(configBuilder, args);

        #region IDisposable

        private bool disposedValue = false;

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
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}