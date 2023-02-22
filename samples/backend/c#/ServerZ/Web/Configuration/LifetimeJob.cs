using System;
using ZzzLab.Web.Middelware;

namespace ZzzLab.Web.Configuration
{
    public class LifetimeJob : ILifetimeJob
    {
        public virtual void OnAppStarted()
        {
            Logger.Info($"App started at {DateTime.Now}");
        }

        public virtual void OnAppStopping()
        {
            Logger.Info($"App Stopping at {DateTime.Now}");
        }

        public virtual void OnAppStopped()
        {
            Logger.Info($"App Stopped at {DateTime.Now}");
        }
    }
}