using Quartz;
using System.Collections.Generic;
using System.Collections.Specialized;
using ZzzLab.Scheduler.Models;

namespace ZzzLab.Scheduler
{
    /// <summary>
    /// 스케줄러 관리자
    /// </summary>
    public static class ScheduleManager
    {
        private static readonly ScheduleBuilder Instance;

        static ScheduleManager()
        {
            NameValueCollection properties = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
                { "quartz.scheduler.instanceName", "MetaIOScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
            };

            Instance = ScheduleBuilder.Create(properties);
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

        public static void PauseJob(string name)
            => Instance.PauseJob(name);

        public static void ResumeJob(string name)
            => Instance.ResumeJob(name);

        public static void ReScheduleJob(string name, string seconds)
            => Instance.ReScheduleJob(name, seconds);

        public static void ReScheduleJob(string name, int cronExpression)
            => Instance.ReScheduleJob(name, cronExpression);

        public static IEnumerable<JobEntiry> GetAllJobs()
             => Instance.GetAllJobs();

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

        #endregion Message
    }
}