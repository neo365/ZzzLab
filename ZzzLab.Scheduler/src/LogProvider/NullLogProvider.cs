using Quartz.Logging;
using System;

namespace ZzzLab.Scheduler
{
    /// <summary>
    /// 아무것도 찍지 말자
    /// </summary>
    public class NullLogProvider : ILogProvider
    {
        /// <summary>
        /// 아무것도 찍지 말자
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Quartz.Logging.Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                // Do Nothjing
                return true;
            };
        }

        /// <summary>
        /// 아무것도 찍지 말자
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 아무것도 찍지 말자
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="destructure"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }
    }
}