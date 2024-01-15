using Quartz;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using ZzzLab.Scheduler.Models;

namespace ZzzLab.Scheduler
{
    /// <summary>
    /// 스케줄러 관리자
    /// </summary>
    public static class ScheduleManager
    {
        private static readonly NameValueCollection Properties = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
                { "quartz.scheduler.instanceName", "MetaIOScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
            };

        private static readonly Lazy<ScheduleBuilder> instance = new Lazy<ScheduleBuilder>(() => ScheduleBuilder.Create(Properties));

        private static ScheduleBuilder Instance
        {
            get => instance.Value;
        }

        public static void AddSchedulerListener(ISchedulerListener listener)
            => Instance.AddSchedulerListener(listener);

        public static void AddJobListener(IJobListener listener)
            => Instance.AddJobListener(listener);

        public static void AddTriggerListener(ITriggerListener listener)
            => Instance.AddTriggerListener(listener);

        public static void AddJob<T>(int seconds) where T : IJobSchedule
            => Instance.AddJob<T>(seconds);

        public static void AddJob<T>(string cronExpression) where T : IJobSchedule
            => Instance.AddJob<T>(cronExpression);

        public static void PauseJob(string key)
            => Instance.PauseJob(key);

        public static void ResumeJob(string key)
            => Instance.ResumeJob(key);

        public static void ReScheduleJob(string key, string seconds)
            => Instance.ReScheduleJob(key, seconds);

        public static void ReScheduleJob(string key, int cronExpression)
            => Instance.ReScheduleJob(key, cronExpression);

        public static IEnumerable<JobEntiry> GetAllJobs()
        {
            IEnumerable<JobEntiry> jobList =  Instance.GetAllJobs();

            foreach (JobEntiry job in jobList) { 
                job.Name = Instance.JobList.Find( x => x.Key.EqualsIgnoreCase(job.Key))?.Name;
            }

            return jobList;
        }

        public static JobEntiry GetJob(string key)
        {
            JobEntiry job = Instance.GetJob(key);
            job.Name = Instance.JobList.Find(x => x.Key.EqualsIgnoreCase(job.Key))?.Name;

            return job;
        }

        /// <summary>
        /// 스케쥴러 끄기
        /// </summary>
        /// <param name="waitForJobsToComplete"></param>
        public static void Shutdown(bool waitForJobsToComplete = false)
            => Instance.Shutdown(waitForJobsToComplete);

        #region Logger

        public delegate void ConsoleWriterDelegate(string message);

        public static ConsoleWriterDelegate Message;

        private static string TempMessage = string.Empty;

        internal static void ConsoleClear()
            => TempMessage = string.Empty;

        internal static void ConsoleWrite(string message)
            => TempMessage += message;

        internal static void ConsoleWriteLine(string message)
        {
            ConsoleWrite(message);
            ScheduleManager.Message?.Invoke(TempMessage);
            ConsoleClear();
        }

        #endregion Logger
    }
}