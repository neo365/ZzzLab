using System;
using System.IO;
using System.Text;

namespace ZzzLab.Crypt
{
    public abstract class CryptBase : ICrypt
    {
        public virtual string Algorithm { get; }

        protected CryptBase(string algorithm)
        {
            Algorithm = algorithm;
        }

        #region Encrypt

        public virtual string EncryptToString(Stream stream)
            => Convert.ToBase64String(EncryptToBytes(stream));

        public virtual string EncryptToString(byte[] bytes)
            => Convert.ToBase64String(EncryptToBytes(bytes));

        public virtual string EncryptToString(string s, Encoding encoding = null)
            => Convert.ToBase64String(EncryptToBytes(s, encoding));

        public abstract byte[] EncryptToBytes(Stream stream);

        public virtual byte[] EncryptToBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return EncryptToBytes(ms);
            }
        }

        public virtual byte[] EncryptToBytes(string s, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;
            return EncryptToBytes(encoding.GetBytes(s));
        }

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
            => EncryptToString(s).EncodeUrlSafeString();

        public virtual string DecryptUrlSafe(string s)
            => Encoding.Default.GetString(DecryptToBytes(s.DecodeUrlSafeString()));



        #endregion URL Safe
    }
}