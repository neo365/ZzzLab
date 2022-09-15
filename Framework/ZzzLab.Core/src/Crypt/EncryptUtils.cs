using System;
using System.Threading;

namespace ZzzLab.Crypt
{
    public static partial class EncryptUtils
    {
        public static string CreateGuid()
        => Guid.NewGuid().ToString();

        public static long GetCustomTimeTick()
        {
            long orig, newval;
            long lastTimeStamp = DateTime.UtcNow.Ticks;
            do
            {
                orig = DateTime.UtcNow.Ticks;
                long now = DateTime.UtcNow.Ticks;
                newval = Math.Max(now, orig + 1);
            } while (Interlocked.CompareExchange(ref lastTimeStamp, newval, orig) != orig);

            return newval;
        }

        public static string GetCustomTimeTickCode()
            => GetCustomTimeTick().ToString();
    }
}