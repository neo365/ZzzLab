﻿using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using ZzzLab.Scheduler.Models;

namespace ZzzLab.Scheduler
{
    /// <summary>
    ///
    /// </summary>
    internal class ScheduleBuilder
    {
        private readonly IScheduler Scheduler;

        public NameValueCollection Properties { get; }

        private ScheduleBuilder(NameValueCollection properties = null)
        {
            // 이거 안해주면 log4net 쓸때 미친듯이 로그찍힘
            LogProvider.SetCurrentLogProvider(new NullLogProvider());

            this.Properties = properties;

            if (this.Properties == null || this.Properties.Count == 0)
            {
            }

            StdSchedulerFactory factory = new StdSchedulerFactory(this.Properties);
            this.Scheduler = factory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();
            this.Scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static ScheduleBuilder Create(NameValueCollection props = null)
            => new ScheduleBuilder(props);

        internal void Shutdown(bool waitForJobsToComplete = false)
            => this.Scheduler.Shutdown(waitForJobsToComplete).ConfigureAwait(false).GetAwaiter().GetResult();

        private IJobDetail BuildJobDetail<T>(string group, string name, string description) where T : IJobSchedule
        {
            JobBuilder builder = JobBuilder.Create<T>();

            if (string.IsNullOrWhiteSpace(group)) builder.WithIdentity(name, group);
            else builder.WithIdentity(name);

            if (string.IsNullOrWhiteSpace(description)) builder.WithDescription(description);

            return builder.Build();
        }

        private ITrigger BuildTrigger(int seconds)
        {
            return TriggerBuilder.Create()
                                 .WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds).RepeatForever())
                                 .Build();
        }

        private ICronTrigger BuildTrigger(string cronExpression)
        {
            return (ICronTrigger)TriggerBuilder.Create()
                                               .WithCronSchedule(cronExpression)
                                               .Build();
        }

        internal void AddJob<T>(int seconds) where T : IJobSchedule
        {
            if (Activator.CreateInstance(typeof(T)) is IJobSchedule job)
            {
                IJobDetail jobDetail = BuildJobDetail<T>(job.Group, job.Name, job.Description);

                if (Task.Run(() => Scheduler.CheckExists(jobDetail.Key)).Result) throw new DuplicateItemException(job.Name);

                Scheduler.ScheduleJob(jobDetail, BuildTrigger(seconds)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else throw new InvalidTypeException(typeof(T));
        }

        internal void AddJob<T>(string cronExpression) where T : IJobSchedule
        {
            if (Activator.CreateInstance(typeof(T)) is IJobSchedule job)
            {
                IJobDetail jobDetail = BuildJobDetail<T>(job.Group, job.Name, job.Description);

                if (Task.Run(() => Scheduler.CheckExists(jobDetail.Key)).Result) throw new DuplicateItemException(job.Name);

                Scheduler.ScheduleJob(jobDetail, BuildTrigger(cronExpression)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else throw new InvalidTypeException(typeof(T));
        }

        internal void PauseJob(string key)
            => this.Scheduler.PauseJob(new JobKey(key));

        internal void ResumeJob(string key)
            => this.Scheduler.ResumeJob(new JobKey(key));

        internal void ReScheduleJob(string key, string cronExpression)
        {
            ITrigger oldTrigger = this.Scheduler.GetTriggersOfJob(new JobKey(key)).ConfigureAwait(false).GetAwaiter().GetResult().FirstOrDefault();
            Scheduler.RescheduleJob(oldTrigger.Key, BuildTrigger(cronExpression));
        }

        internal void ReScheduleJob(string name, int seconds)
        {
            ITrigger oldTrigger = this.Scheduler.GetTriggersOfJob(new JobKey(name)).ConfigureAwait(false).GetAwaiter().GetResult().FirstOrDefault();
            Scheduler.RescheduleJob(oldTrigger.Key, BuildTrigger(seconds));
        }

        internal void DeleteJob(string key)
            => Scheduler.DeleteJob(new JobKey(key));

        public void AddSchedulerListener(ISchedulerListener listener)
            => this.Scheduler.ListenerManager.AddSchedulerListener(listener);

        public void AddJobListener(IJobListener listener)
            => this.Scheduler.ListenerManager.AddJobListener(listener);

        public void AddTriggerListener(ITriggerListener listener)
            => this.Scheduler.ListenerManager.AddTriggerListener(listener);

        public IEnumerable<string> GetAllGroups()
            => this.Scheduler.GetAllGroups();

        public IEnumerable<JobEntiry> GetAllJobs()
            => this.Scheduler.GetAllJobs();
    }
}