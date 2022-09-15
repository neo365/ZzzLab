using Quartz;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ZzzLab.Scheduler.Listener
{
    public class JobListenerBase : IJobListener
    {
        public virtual string Name => "JobListener";

        public virtual Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"JobExecutionVetoed: {context.JobDetail.Key.Name}");
            return Task.CompletedTask;
        }

        public virtual Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"JobToBeExecuted: {context.JobDetail.Key.Name}");

            return Task.CompletedTask;
        }

        public virtual Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"JobWasExecuted: {context.JobDetail.Key.Name}");

            return Task.CompletedTask;
        }
    }
}