using ZzzLab.AspCore.ScheduleTask;
using ZzzLab.Scheduler;
using ZzzLab.Web.Middelware;

namespace ZzzLab.AspCore.Configuration
{
    /// <summary>
    /// 구 ASP.net의 Global.asx 같은 역할을 하는 모듈
    /// </summary>
    public sealed class LifetimeJob : ILifetimeJob
    {
        public void OnAppStarted()
        {
            ScheduleManager.AddSimpleJob<SessionJob>("SessionJob", "SessionJob", 60);
        }

        public void OnAppStopping()
        {
            ScheduleManager.Shutdown();
        }

        public void OnAppStopped()
        {
        }
    }
}