using Quartz;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ZzzLab.Scheduler.Listener
{
    public class SchedulerListenerBase : ISchedulerListener
    {
        public virtual Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobAdded: {jobDetail}");
            return Task.CompletedTask;
        }

        public virtual Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobDeleted: {jobKey}");
            return Task.CompletedTask;
        }

        public virtual Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobInterrupted: {jobKey}");
            return Task.CompletedTask;
        }

        public virtual Task JobPaused(JobKey jobKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobInterrupted: {jobKey}");
            return Task.CompletedTask;
        }

        public virtual Task JobResumed(JobKey jobKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobResumed: {jobKey}");
            return Task.CompletedTask;
        }

        public virtual Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobScheduled: {trigger}");
            return Task.CompletedTask;
        }

        public virtual Task JobsPaused(string jobGroup, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobsPaused: {jobGroup}");
            return Task.CompletedTask;
        }

        public virtual Task JobsResumed(string jobGroup, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobsResumed: {jobGroup}");
            return Task.CompletedTask;
        }

        public virtual Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"JobUnscheduled: {triggerKey}");
            return Task.CompletedTask;
        }

        public virtual Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulerError: {msg}");
            return Task.CompletedTask;
        }

        public virtual Task SchedulerInStandbyMode(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulerInStandbyMode");
            return Task.CompletedTask;
        }

        public virtual Task SchedulerStarting(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulerStarting");
            return Task.CompletedTask;
        }

        public virtual Task SchedulerStarted(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulerStarted");
            return Task.CompletedTask;
        }

        public virtual Task SchedulerShuttingdown(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulerShuttingdown");
            return Task.CompletedTask;
        }

        public virtual Task SchedulerShutdown(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulerShutdown");
            return Task.CompletedTask;
        }

        public virtual Task SchedulingDataCleared(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"SchedulingDataCleared");
            return Task.CompletedTask;
        }

        public virtual Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"TriggerFinalized: {trigger}");
            return Task.CompletedTask;
        }

        public virtual Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"TriggerPaused: {triggerKey}");
            return Task.CompletedTask;
        }

        public virtual Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"TriggerResumed: {triggerKey}");
            return Task.CompletedTask;
        }

        public virtual Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"TriggersPaused: {triggerGroup}");
            return Task.CompletedTask;
        }

        public virtual Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"TriggersResumed: {triggerGroup}");
            return Task.CompletedTask;
        }
    }
}