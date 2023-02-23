using Microsoft.Extensions.Logging;
using ZzzLab.Logging;

namespace ZzzLab
{
    internal class ScheduleLogger : PrintLogger
    {
        public override string Name => "Schedule";

        public override LogLevel PrintLevel { set; get; } = LogLevel.Debug;
    }
}
