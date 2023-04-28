using System;
using System.Linq;
using ZzzLab.Configuration;

namespace ZzzLab.Data.Configuration
{
    internal class DBClientBuilder : IServiceBuilder, IDisposable
    {
        internal static SQLCollection Queries { set; get; }

        internal static IDBConfigurationLoader BaseReader { get; set; }

        internal DBClientBuilder(IDBConfigurationLoader reader)
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
                Configurator.Setting.DBConnector = new ConnectionCollection();
                Configurator.Setting.DBQueries = new SQLCollection();

                if (BaseReader == null) return;

                var configs = BaseReader.Reader();

                if (configs != null && configs.Any())
                {
                    foreach (var item in configs)
                    {
                        Configurator.Setting.DBConnector.Add(item);
                    }
                }

                var collection = BaseReader.QueryReader();

                var sqlCollection = new SQLCollection();
                if (collection != null && collection.Any())
                {
                    foreach (var item in collection)
                    {
                        sqlCollection.Add(item);
                    }

                    Configurator.Setting.DBQueries = sqlCollection;
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