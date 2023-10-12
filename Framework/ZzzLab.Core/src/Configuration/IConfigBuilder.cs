using System;
using ZzzLab.Logging;

namespace ZzzLab.Configuration
{
    public interface IConfigBuilder : IDisposable
    {
        IConfigBuilder Use(IServiceBuilder serviceBuilder, params object[] args);

        IConfigBuilder Use<T>(params object[] args) where T : IServiceBuilder;

        IConfigBuilder AddLogger(IZLogger logger);

        IConfigBuilder AddLogger<T>(string name, LogEventHandler onMessage) where T : IZLogger;

        IConfigBuilder RemoveLogger(IZLogger logger);
    }
}