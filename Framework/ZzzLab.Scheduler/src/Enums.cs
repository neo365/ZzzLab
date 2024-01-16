namespace ZzzLab.Scheduler
{
    public enum ScheduleStatus : int
    {
        None = 0,
        Started = 1,
        Standby = 2,
        Shutdown = 3,
    }

    public enum JobType : int
    {
        None = 0,
        Simple = 1,
        Cron = 2,
    }
}