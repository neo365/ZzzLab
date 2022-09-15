using Quartz;
using System;

namespace ZzzLab.Scheduler
{
    internal static class TriggerExtensions
    {
        public static DateTime GetStartTime(this ITrigger trigger)
            => trigger.StartTimeUtc.LocalDateTime;

        public static DateTime? GetEndTime(this ITrigger trigger)
            => trigger.EndTimeUtc?.ToDateTimeNullable();

        public static DateTime? GetFinalFireTime(this ITrigger trigger)
            => trigger.FinalFireTimeUtc?.ToDateTimeNullable();

        public static DateTime? GetPreviousFireTime(this ITrigger trigger)
            => trigger.GetPreviousFireTimeUtc()?.ToDateTimeNullable();

        public static DateTime? GetNextFireTime(this ITrigger trigger)
            => trigger.GetNextFireTimeUtc()?.ToDateTimeNullable();

        public static DateTime? GetFireTimeAfter(this ITrigger trigger, DateTime afterTime)
            => trigger.GetFireTimeAfter(afterTime.ToDateTimeOffset())?.ToDateTimeNullable();

        public static JobType GetTriggerType(this ITrigger trigger)
        {
            if (trigger is ICronTrigger) return JobType.Cron;
            else if (trigger is ISimpleTrigger) return JobType.Simple;
            return JobType.None;
        }

        public static string GetCronExpression(this ITrigger trigger)
        {
            if (trigger is ICronTrigger cronTrigger) return cronTrigger.CronExpressionString;
            else return string.Empty;
        }

        public static TimeSpan GetInterval(this ITrigger trigger)
        {
            if (trigger is ISimpleTrigger simpleTrigger) return simpleTrigger.RepeatInterval;

            return TimeSpan.Zero;
        }

        public static string GetIntervalSetting(this ITrigger trigger)
        {
            if (trigger is ISimpleTrigger) return GetInterval(trigger).ToString();
            else if (trigger is ICronTrigger) return GetCronExpression(trigger);

            return string.Empty;
        }
    }
}