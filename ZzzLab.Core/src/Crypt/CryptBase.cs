using System;
using System.IO;
using System.Text;

namespace ZzzLab.Crypt
{
    public abstract class CryptBase
    {
        public string Algorithm { get; }

        protected CryptBase(string algorithm)
        {
            Algorithm = algorithm;
        }

        #region Encrypt

        public virtual string EncryptToString(Stream stream)
            => Convert.ToBase64String(EncryptToBytes(stream));

        public virtual string EncryptToString(byte[] bytes)
            => Convert.ToBase64String(EncryptToBytes(bytes));

        public virtual string EncryptToString(string s)
            => Convert.ToBase64String(EncryptToBytes(s));

        public abstract byte[] EncryptToBytes(Stream stream);

        public virtual byte[] EncryptToBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return EncryptToBytes(ms);
            }
        }

        public virtual byte[] EncryptToBytes(string s)
            => EncryptToBytes(Encoding.Default.GetBytes(s));

        #endregion Encrypt

        #region Decrypt

        public virtual string DecryptToString(Stream stream)
            => Encoding.Default.GetString(DecryptToBytes(stream));

        public virtual string DecryptToString(byte[] bytes)
            => Encoding.Default.GetString(DecryptToBytes(bytes));

        public virtual string DecryptToString(string s)
            => Encoding.Default.GetString(DecryptToBytes(s));

        public abstract byte[] DecryptToBytes(Stream stream);

        public virtual byte[] DecryptToBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return EncryptToBytes(ms);
            }
        }

        public virtual byte[] DecryptToBytes(string s)
            => EncryptToBytes(Convert.FromBase64String(s));

        #endregion Decrypt

        #region URL Safe

        public virtual string EncryptUrlSafe(string s)
            => EncodeUrlSafeString(EncryptToString(s));

        public virtual string DecryptUrlSafe(string s)
            => Encoding.Default.GetString(DecryptToBytes(DecodeUrlSafeString(s)));

        protected virtual string EncodeUrlSafeString(string s)
            => s.Replace("=", ",").Replace("+", "-").Replace("/", "_");

        protected virtual string DecodeUrlSafeString(string s)
            => s.Replace(",", "=").Replace("-", "+").Replace("_", "/");

        #endregion URL Safe
    }
}