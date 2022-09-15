using Quartz;
using ZzzLab;
using ZzzLab.Scheduler;

namespace ConsoleSample
{
    internal class Test01Job : JobScheduleBase, IJobSchedule
    {
        public override string Key => "C4BD1223-59BA-4D48-A024-0ECE7C28C017";

        public override string Name => "테스트 job 01";

        public override Task Execute(IJobExecutionContext context)
        {
            Logger.Info($"[{this.Name}] Call Execute");

            return Task.CompletedTask;
        }
    }
}