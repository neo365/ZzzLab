using System;
using ZzzLab.Configuration;

namespace ZzzLab.Data.Configuration
{
    public static class DBClientBuilderExtention
    {
        public static IConfigBuilder UseDBClient<T>(this IConfigBuilder configBuilder) where T : IDBConfigurationLoader
        {
            if (Activator.CreateInstance(typeof(T)) is IDBConfigurationLoader reader)
            {
                configBuilder.Use(new DBClientBuilder(reader));
            }
            else throw new InvalidTypeException(typeof(T));

            return configBuilder;
        }

        public static IConfigBuilder UseDBClient(this IConfigBuilder configBuilder, IDBConfigurationLoader reader)
        {
            configBuilder.Use(new DBClientBuilder(reader));
            return configBuilder;
        }
    }
}