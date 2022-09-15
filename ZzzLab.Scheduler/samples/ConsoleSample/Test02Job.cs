using Quartz;
using ZzzLab;
using ZzzLab.Logging;
using ZzzLab.Scheduler;

namespace ConsoleSample
{
    internal class Test02Job : JobScheduleBase, IJobSchedule
    {
        public override string Key => "F2BA445A-4A36-4465-88C7-816A5018DB71";

        public override string Name => "테스트 job 02";

        public override Task Execute(IJobExecutionContext context)
        {
            Logger.Info($"[{this.Name}] Call Execute");

            return Task.CompletedTask;
        }
    }
}