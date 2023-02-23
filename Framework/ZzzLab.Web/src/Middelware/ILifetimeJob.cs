using Microsoft.Extensions.Hosting;

namespace ZzzLab.Web.Middelware
{
    public interface ILifetimeJob
    {
        IHostApplicationLifetime? Lifetime { get; }

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
    }
}