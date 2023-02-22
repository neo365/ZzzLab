using System;
using System.ComponentModel;
using ZzzLab.Event;
using ZzzLab.Web.Middelware;

namespace ZzzLab.Web.Configuration
{
    public class LifetimeJob : ILifetimeJob
    {
        public virtual void OnAppStarted() => this.OnStarted();

        public virtual void OnAppStopping() => this.OnStopping();

        public virtual void OnAppStopped() => this.OnStopped();

        #region EVENT

        public event ServerStatusChangeEventHandler? StatusChanged;

        #region Synchronizing

        private ISynchronizeInvoke? _SynchronizingObject;

        [Browsable(false), DefaultValue(null)]
        protected ISynchronizeInvoke? SynchronizingObject
        {
            get { return this._SynchronizingObject; }
            set { this._SynchronizingObject = value; }
        }

        #endregion Synchronizing

        #region Fire Event

        protected virtual void OnStarted(string? message = null)
            => OnStatusChanged(new ServerStatusChangeEventArgs(ServerStatus.Started, message));

        protected virtual void OnStopping(string? message = null)
            => OnStatusChanged(new ServerStatusChangeEventArgs(ServerStatus.Stopping, message));

        protected virtual void OnStopped(string? message = null)
            => OnStatusChanged(new ServerStatusChangeEventArgs(ServerStatus.Stoped, message));

        protected virtual void OnStatusChanged(ServerStatusChangeEventArgs e)
        {
            Logger.Info($"Web {e.Status} at {DateTime.Now}");

            if (this.StatusChanged != null)
            {
                if ((this.SynchronizingObject != null) && this.SynchronizingObject.InvokeRequired)
                {
                    this.SynchronizingObject.BeginInvoke(StatusChanged, new object[] { this, e });
                }
                else StatusChanged(this, e);
            }
        }

        #endregion Fire Event

        #endregion EVENT
    }
}