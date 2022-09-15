using Quartz;
using ZzzLab.Scheduler;
using ILogger = ZzzLab.Logging.ILogger;

namespace ZzzLab.AspCore.ScheduleTask
{
    internal class SessionJob : JobScheduleBase, IJobSchedule
    {
        public override ILogger Logger => ScheduleHelper.Logger;
        public override bool Sync(IJobExecutionContext context)
        {
            return true;
        }
    }
}