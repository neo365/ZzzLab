using Quartz.Logging;
using System;

namespace ZzzLab.Scheduler
{
    /// <summary>
    /// Quartz 로그를 콘솔로 출력하기 위한 Provider
    /// </summary>
    public class ConsoleLogProvider : ILogProvider
    {
        /// <summary>
        /// Gets the specified named logger.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        /// <returns>The logger reference.</returns>
        public Quartz.Logging.Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= LogLevel.Info && func != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                }
                return true;
            };
        }

        /// <summary>
        /// Opens a nested diagnostics context. Not supported in EntLib logging. 미구현
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IDisposable OpenNestedContext(string message)
            => throw new NotImplementedException();

        /// <summary>
        /// Opens a mapped diagnostics context. Not supported in EntLib logging.  미구현
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="destructure"></param>
        /// <returns></returns>
        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            => throw new NotImplementedException();
    }
}