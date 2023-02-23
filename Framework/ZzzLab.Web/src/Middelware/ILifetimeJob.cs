using Microsoft.Extensions.Hosting;
using ZzzLab.Event;

namespace ZzzLab.Web.Middelware
{
    public interface ILifetimeJob
    {
        IHostApplicationLifetime? Lifetime { get; }

        event ServerStatusChangeEventHandler? StatusChanged;

        /// <summary>
        /// 웹앱이 시작될때 호출된다.
        /// </summary>
        void OnAppStarted();

        /// <summary>
        /// 웹앱이 종료를 시작할때 호출된다.
        /// </summary>
        void OnAppStopped();

        /// <summary>
        /// 웹앱이 종료 할때 호출된다.
        /// </summary>
        void OnAppStopping();

        /// <summary>
        /// 웹앱을 종료한다.
        /// </summary>
        public void Stop();
    }
}