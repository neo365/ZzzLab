using System;

namespace ZzzLab.Configuration
{
    public interface IServiceBuilder : IDisposable
    {
        void Initialize(IConfigBuilder configBuilder, params object[] args);

        void Load(IConfigBuilder configBuilder, params object[] args);

        void ReLoad(IConfigBuilder configBuilder, params object[] args);
    }
}