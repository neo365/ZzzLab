using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quartz;
using System;

namespace ZzzLab.Scheduler.Models
{
    public class JobEntiry
    {
        public string Key { set; get; }
        public string Group { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TriggerState Status { set; get; } = TriggerState.None;

        [JsonConverter(typeof(StringEnumConverter))]
        public JobType JobType { set; get; }

        public string Interval { set; get; }
        public DateTime StartedTime { set; get; }
        public DateTime? PreviousFireTime { set; get; }
        public DateTime? NextFireTime { set; get; }
    }
}