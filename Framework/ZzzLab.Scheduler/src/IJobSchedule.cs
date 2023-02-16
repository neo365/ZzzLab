using Quartz;

namespace ZzzLab.Scheduler
{
    public interface IJobSchedule : IJob
    {
        /// <summary>
        /// 스케쥴 고유번호
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 스케쥴 그룹
        /// </summary>
        string Group { get; }

        /// <summary>
        /// 스케쥴명
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 스케쥴설명
        /// </summary>
        string Description { get; }
    }
}