using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ZzzLab.Scheduler.Models
{
    public class JobEntiry
    {
        public string Group { set; get; }
        public string Key { set; get; }
        public string Description { set; get; }
        public string Status { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public JobType JobType { set; get; }

        public string Interval { set; get; }
        public DateTime StartedTime { set; get; }
        public DateTime? PreviousFireTime { set; get; }
        public DateTime? NextFireTime { set; get; }
    }
}