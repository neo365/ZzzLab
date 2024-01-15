using Quartz;
using Quartz.Impl.Matchers;
using System.Collections.Generic;
using System.Linq;
using ZzzLab.Scheduler.Models;

namespace ZzzLab.Scheduler
{
    public static class JobExtensions
    {
        public static IEnumerable<string> GetAllGroups(this IScheduler scheduler)
            => scheduler.GetJobGroupNames().Result;

        public static IEnumerable<JobEntiry> GetAllJobs(this IScheduler scheduler)
        {
            List<JobEntiry> jobs = new List<JobEntiry>();

            foreach (string groupName in GetAllGroups(scheduler))
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(groupName);

                foreach (JobKey jobKey in scheduler.GetJobKeys(groupMatcher).Result)
                {
                    ITrigger trigger = scheduler.GetTriggersOfJob(jobKey).Result.First();
                    TriggerState status = scheduler.GetTriggerState(trigger.Key).ConfigureAwait(false).GetAwaiter().GetResult();
                    IJobDetail detail = scheduler.GetJobDetail(jobKey).ConfigureAwait(false).GetAwaiter().GetResult();

                    JobType jobType = JobType.None;
                    string interval = string.Empty;
                    if (trigger is ISimpleTrigger simpleTrigger)
                    {
                        jobType = JobType.Simple;
                        interval = simpleTrigger.RepeatInterval.ToString();
                    }
                    else if (trigger is ICronTrigger cronTrigger)
                    {
                        jobType = JobType.Cron;
                        interval = cronTrigger.CronExpressionString;
                    }

                    JobEntiry jobEntiry = new JobEntiry
                    {
                        Group = jobKey.Group,
                        Key = jobKey.Name,
                        Description = detail.Description,
                        Status = status.ToString(),
                        JobType = jobType,
                        Interval = interval,
                        StartedTime = trigger.GetStartTime(),
                        PreviousFireTime = trigger.GetPreviousFireTime(),
                        NextFireTime = trigger.GetNextFireTime()
                    };

                    jobs.Add(jobEntiry);
                }
            }
            return jobs;
        }
        public static JobEntiry GetJob(this IScheduler scheduler, string key)
        {
            foreach (string groupName in GetAllGroups(scheduler))
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(groupName);

                foreach (JobKey jobKey in scheduler.GetJobKeys(groupMatcher).Result)
                {
                    if (jobKey.Name.EqualsIgnoreCase(key) == false) continue;
                    ITrigger trigger = scheduler.GetTriggersOfJob(jobKey).Result.First();
                    TriggerState status = scheduler.GetTriggerState(trigger.Key).ConfigureAwait(false).GetAwaiter().GetResult();
                    IJobDetail detail = scheduler.GetJobDetail(jobKey).ConfigureAwait(false).GetAwaiter().GetResult();

                    JobType jobType = JobType.None;
                    string interval = string.Empty;
                    if (trigger is ISimpleTrigger simpleTrigger)
                    {
                        jobType = JobType.Simple;
                        interval = simpleTrigger.RepeatInterval.ToString();
                    }
                    else if (trigger is ICronTrigger cronTrigger)
                    {
                        jobType = JobType.Cron;
                        interval = cronTrigger.CronExpressionString;
                    }

                    JobEntiry jobEntiry = new JobEntiry
                    {
                        Group = jobKey.Group,
                        Key = jobKey.Name,
                        Description = detail.Description,
                        Status = status.ToString(),
                        JobType = jobType,
                        Interval = interval,
                        StartedTime = trigger.GetStartTime(),
                        PreviousFireTime = trigger.GetPreviousFireTime(),
                        NextFireTime = trigger.GetNextFireTime()
                    };

                    return jobEntiry;
                }
            }
            return null;
        }
    }
}