using System;
using ZzzLab.Logging;

namespace ZzzLab.Configuration
{
    public interface IConfigBuilder : IDisposable
    {
        IConfigBuilder Use(IServiceBuilder serviceBuilder, params object[] args);

        IConfigBuilder Use<T>(params object[] args) where T : IServiceBuilder;

        IConfigBuilder AddLogger(ILogger logger);

        IConfigBuilder AddLogger<T>(string name, LogEventHandler onMessage) where T : ILogger;

        IConfigBuilder RemoveLogger(ILogger logger);

        IConfigBuilder AddWatcher(string filePath);

        IConfigBuilder RemoveWatcher(string filePath);
    }
}