using ZzzLab.Crypt;

namespace ZzzLab.Web.Crypt
{
    public static class OtaCrypt
    {
        private const string _Header = "yyyyMMddHHmmssfff";

        public static string Encrypt(string text, string seed)
            => BouncyCastleCrypt.Encrypt($"{DateTime.Now.ToString(_Header)}{text}", seed);

        public static string Decrypt(string text, string seed)
            => BouncyCastleCrypt.Decrypt(text, seed).Substring(_Header.Length);
    }
}