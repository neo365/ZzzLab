using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Diagnostics;

namespace ZzzLab.Logging
{
    public static class LogExtenttion
    {
        public static LogLevel ToLogLevel(this TraceLevel level)
        {
            switch (level)
            {
                case TraceLevel.Off: return LogLevel.None;
                case TraceLevel.Verbose: return LogLevel.Debug;
                case TraceLevel.Info: return LogLevel.Information;
                case TraceLevel.Warning: return LogLevel.Warning;
                case TraceLevel.Error: return LogLevel.Error;
                default:
                    break;
            }

            return LogLevel.Critical;
        }

        public static ILoggingBuilder AddCustomLogger<TClass>(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ZLoggerProvider<TClass>>());

            LoggerProviderOptions.RegisterProviderOptions
                <DummyLoggerConfiguration, ZLoggerProvider<TClass>>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddCustomLogger<TClass>(
            this ILoggingBuilder builder,
            Action<DummyLoggerConfiguration> configure)
        {
            builder.AddCustomLogger<TClass>();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}