namespace System
{
    public static class BitMaskExtension
    {
        public static bool HasMask<T>(this T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static T AddMask<T>(this T flags, params T[] args) where T : struct
        {
            T result = flags;

            foreach (T arg in args)
            {
                int flagsValue = (int)(object)result;
                int flagValue = (int)(object)arg;
                result = (T)(object)(flagsValue | flagValue);
            }

            return (T)(object)result;
        }

        public static T RemoveMask<T>(this T flags, params T[] args) where T : struct
        {
            T result = flags;

            foreach (T arg in args)
            {
                int flagsValue = (int)(object)result;
                int flagValue = (int)(object)arg;
                result = (T)(object)(flagsValue & (~flagValue));
            }

            return (T)(object)result;
        }
    }
}