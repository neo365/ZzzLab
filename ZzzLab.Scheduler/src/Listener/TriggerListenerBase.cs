using Quartz;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ZzzLab.Scheduler.Listener
{
    public class TriggerListenerBase : ITriggerListener
    {
        public virtual string Name => "TriggerListener";

        public virtual Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"TriggerComplete: {trigger.Key}");

            return Task.CompletedTask;
        }

        public virtual Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"TriggerFired: {trigger.Key}");
            return Task.CompletedTask;
        }

        public virtual Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"TriggerMisfired: {trigger.Key}");
            return Task.CompletedTask;
        }

        public virtual Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Debug.WriteLine($"VetoJobExecution: {trigger.Key}");

            return Task.FromResult(false);
        }
    }
}