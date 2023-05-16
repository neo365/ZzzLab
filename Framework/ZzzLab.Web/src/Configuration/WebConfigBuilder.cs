using ZzzLab.Configuration;

namespace ZzzLab.Web.Configuration
{
    internal class WebConfigBuilder : IServiceBuilder, IDisposable
    {
        internal static IWebConfigurationLoader? BaseReader { get; set; }

        internal WebConfigBuilder(IWebConfigurationLoader reader)
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
                Configurator.Setting.WebConfig = new WebConfig();

                if (BaseReader == null) return;

                var configs = BaseReader.Reader();

                if (configs != null && configs.Any())
                {
                    Configurator.Setting.WebConfig = configs.FirstOrDefault();
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