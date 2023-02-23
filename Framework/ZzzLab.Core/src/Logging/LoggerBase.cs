using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ZzzLab.Logging
{
    public abstract class LoggerBase : ILogger
    {
        public virtual string Name { get; } = Assembly.GetExecutingAssembly().GetName().Name;

        public virtual LogLevel PrintLevel { set; get; } = LogLevel.Debug;

        protected LoggerBase()
        {
        }

        protected LoggerBase(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name) == false) Name = name;
        }

        public virtual void Debug(object value, [CallerMemberName] string methodName = null)
            => Log(LogLevel.Debug, value, methodName);

        public virtual void Info(object value, [CallerMemberName] string methodName = null)
            => Log(LogLevel.Information, value, methodName);

        public virtual void Warning(object value, [CallerMemberName] string methodName = null)
            => Log(LogLevel.Warning, value, methodName);

        public virtual void Error(object value, [CallerMemberName] string methodName = null)
            => Log(LogLevel.Error, value, methodName);

        public virtual void Fatal(object value, [CallerMemberName] string methodName = null)
            => Log(LogLevel.Critical, value, methodName);

        public virtual void Critical(object value, [CallerMemberName] string methodName = null)
            => Log(LogLevel.Critical, value, methodName);

        public abstract void Log(LogLevel level,
                    object value,
                    [CallerMemberName] string methodName = null);

        public virtual void Log(TraceLevel level, object value, [CallerMemberName] string methodName = null)
            => Log(level.ToLogLevel(), value, methodName);

        #region EVENT

        public event LogEventHandler Message;

        #region Synchronizing

        private ISynchronizeInvoke _SynchronizingObject;

        [Browsable(false), DefaultValue(null)]
        protected ISynchronizeInvoke SynchronizingObject
        {
            get { return this._SynchronizingObject; }
            set { this._SynchronizingObject = value; }
        }

        #endregion Synchronizing

        #region Fire Event

        protected virtual void OnMessage(LogLevel level, string methodName, object value, DateTime? logDateTime = null)
        {
            OnMessage(new LogEventArgs(this.Name, level, methodName, value, logDateTime));
        }

        protected virtual void OnMessage(LogEventArgs e)
        {
            if (this.Message != null)
            {
                if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired)
                {
                    this.SynchronizingObject.BeginInvoke(Message, new object[] { this, e });
                }
                else Message(this, e);
            }
        }

        #endregion Fire Event

        #endregion EVENT
    }
}