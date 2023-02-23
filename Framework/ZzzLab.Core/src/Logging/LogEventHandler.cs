using Microsoft.Extensions.Logging;
using System;

namespace ZzzLab.Logging
{
    public delegate void LogAppender(object sender, ILogger e);

    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public class LogEventArgs : EventArgs
    {
        public string Name { get; }
        public LogLevel Level { get; }
        public string MethodName { get; }
        public object Value { get; }
        public DateTime LogDateTime { get; }

        public LogEventArgs(string name, LogLevel level, string methodName, object value, DateTime? logDateTime = null) : base()
        {
            this.Name = name;
            this.Level = level;
            this.MethodName = methodName;
            this.Value = value;
            this.LogDateTime = logDateTime ?? DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Name} | {LogDateTime.To24Hours()} | {Level,-7} | {MethodName,-50} | {Value}";
        }
    }
}