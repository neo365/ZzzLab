using Microsoft.Extensions.Hosting;
using ZzzLab.Web.Middelware;

namespace ZzzLab.Web.Builder
{
    public static partial class WebBuilderExtensions
    {
        /// <summary>
        /// asp.net 4.x 이하 버젼의 Global.asx와 같은 역할을 한다.
        /// </summary>
        /// <typeparam name="T">ILifetimeJob</typeparam>
        /// <param name="lifetime">IHostApplicationLifetime</param>
        /// <returns>IHostApplicationLifetime</returns>
        /// <exception cref="ArgumentNullException">IHostApplicationLifetime</exception>
        /// <exception cref="InvalidTypeException">ILifetimeJob 이 아닐경우</exception>
        public static ILifetimeJob UseLifetime<T>(this IHostApplicationLifetime lifetime)
        {
            if (lifetime == null) throw new ArgumentNullException(nameof(lifetime));

            if (Activator.CreateInstance(typeof(T)) is ILifetimeJob job)
            {
                lifetime.ApplicationStarted.Register(job.OnAppStarted);
                lifetime.ApplicationStopping.Register(job.OnAppStopping);
                lifetime.ApplicationStopped.Register(job.OnAppStopped);
            }
            else throw new InvalidTypeException(typeof(T));

            return job;
        }
    }
}