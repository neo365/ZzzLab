using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZzzLab.Logging;

namespace ZzzLab.Configuration
{
    public partial class ConfigBuilder : IConfigBuilder
    {
        private readonly IConfigurationLoader<KeyValuePair<string, string>> BaseLoader;

        internal ConfigBuilderDelegate Reloader;
        internal NoReturnDelegate Disposer;

        internal ExpandObj Setting = new ExpandObj();
        internal ExpandObj Global = new ExpandObj();

        internal ConfigBuilder()
        {
            Reloader += ReLoad;
            Disposer += Dispose;
        }

        internal ConfigBuilder(IConfigurationLoader<KeyValuePair<string, string>> loader) : this()
        {
            this.BaseLoader = loader;
            this.Load();
        }

        public void Load(params object[] args)
        {
            this.Global = new ExpandObj();

            if (BaseLoader == null) return;

            IEnumerable<KeyValuePair<string, string>> settings = BaseLoader.Reader();

            if (settings == null) return;

            foreach (KeyValuePair<string, string> item in settings)
            {
                this.Global[item.Key] = item.Value;
            }

            if (BaseLoader.WatchFiles != null && BaseLoader.WatchFiles.Any())
            {
                foreach (string filePath in BaseLoader.WatchFiles)
                {
                    if (File.Exists(filePath) == false) continue;

                    AddWatcher(filePath);
                }
            }
        }

        public void Writer(string name, string value)
        {
            if (this.Global == null) this.Global = new ExpandObj();

            if (BaseLoader == null) return;

            this.Global[name] = value;

            foreach (string f in BaseLoader.WatchFiles)
            {
                if (DicWatcher.ContainsKey(f)) DicWatcher[f].EnableRaisingEvents = false;
            }

            BaseLoader.Writer(new KeyValuePair<string, string>(name, value));

            foreach (string f in BaseLoader.WatchFiles)
            {
                if (DicWatcher.ContainsKey(f)) DicWatcher[f].EnableRaisingEvents = true;
            }
        }

        #region IConfigBuilder

        public IConfigBuilder Use(IServiceBuilder serviceBuilder, params object[] args)
        {
            serviceBuilder.Initialize(this, args);
            serviceBuilder.Load(this, args);

            Reloader += serviceBuilder.ReLoad;

            return this;
        }

        public IConfigBuilder Use<T>(params object[] args) where T : IServiceBuilder
        {
            Logger.Debug("Use<T>(params object[] args)");

            if (Activator.CreateInstance(typeof(T)) is IServiceBuilder builder)
            {
                Use(builder, args);
            }

            return this;
        }

        #region Logger

        /// <summary>
        /// 전역로거 추가.  구현 로직상 어쩔수 없이 로거간의 시간차가 존재하게 된다.
        /// </summary>
        /// <param name="logger">IZLogger</param>
        /// <exception cref="InitializeException">초기화가 되지 않았을 경우</exception>
        public IConfigBuilder AddLogger(IZLogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            Logger.DebugFunc += logger.Debug;
            Logger.InfoFunc += logger.Info;
            Logger.WarningFunc += logger.Warning;
            Logger.ErrorFunc += logger.Error;
            Logger.FatalFunc += logger.Fatal;

            return this;
        }

        public IConfigBuilder AddLogger<T>(string name, LogEventHandler onMessage) where T : IZLogger
        {
            if (Activator.CreateInstance(typeof(T), name) is IZLogger logger)
            {
                if (onMessage != null) logger.Message += onMessage;

                Logger.DebugFunc += logger.Debug;
                Logger.InfoFunc += logger.Info;
                Logger.WarningFunc += logger.Warning;
                Logger.ErrorFunc += logger.Error;
                Logger.FatalFunc += logger.Fatal;
            }

            return this;
        }

        /// <summary>
        /// 전역로거 제거
        /// </summary>
        /// <param name="logger">IZLogger</param>
        /// <exception cref="InitializeException">초기화가 되지 않았을 경우</exception>
        public IConfigBuilder RemoveLogger(IZLogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            Logger.DebugFunc -= logger.Debug;
            Logger.InfoFunc -= logger.Info;
            Logger.WarningFunc -= logger.Warning;
            Logger.ErrorFunc -= logger.Error;
            Logger.FatalFunc -= logger.Fatal;

            return this;
        }

        #endregion Logger

        #endregion IConfigBuilder

        public void ReLoad(IConfigBuilder builder, params object[] args)
            => Load(args);

        internal void Reloading(params object[] args)
            => Reloader?.Invoke(this, args);

        internal void Disposing()
            => Disposer?.Invoke();

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}