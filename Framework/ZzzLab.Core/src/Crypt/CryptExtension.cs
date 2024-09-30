namespace ZzzLab.Crypt
{
    public static class CryptExtension
    {
        public static string EncodeUrlSafeString(this string s)
            => s.Replace("=", ",").Replace("+", "-").Replace("/", "_");

        public static string DecodeUrlSafeString(this string s)
            => s.Replace(",", "=").Replace("-", "+").Replace("_", "/");
    }
}