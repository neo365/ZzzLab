using Quartz;
using System.Threading.Tasks;

namespace ZzzLab.Scheduler
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public abstract class JobScheduleBase : IJobSchedule, IJob
    {
        public abstract string Key { get; }

        public virtual string Group { get; } = "Default";

        public abstract string Name { get; }

        public virtual string Description { get; } = string.Empty;

        /// <summary>
        /// IJob 구현 코드<br />
        /// 되도록 이면 손대지 말고 Sync 부분만 구현하자.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <returns></returns>
        public abstract Task Execute(IJobExecutionContext context);
    }
}