using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZzzLab.Crypt
{
    public class HMACSHA512Crypt
    {
        /// <summary>
        /// HAMCSHA256Crypt Signning
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">원본</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptToBytes(byte[] key, Stream stream)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek) stream.Position = 0;
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(stream);
            }
        }

        /// <summary>
        /// HAMCSHA256Crypt Signning
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">원본</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(byte[] key, Stream stream)
            => ByteToHashString(EncryptToBytes(key, stream));

        public static string Encrypt(string key, Stream strem)
            => Encrypt(Encoding.Default.GetBytes(key), strem);

        public static string Encrypt(byte[] key, byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(key));

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Encrypt(key, ms);
            }
        }

        public static string Encrypt(string key, byte[] bytes)
            => Encrypt(Encoding.Default.GetBytes(key), bytes);

        public static string Encrypt(byte[] key, string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(key));

            encoding = encoding ?? Encoding.Default;

            return Encrypt(key, encoding.GetBytes(value));
        }

        public static string Encrypt(string key, string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(key));

            encoding = encoding ?? Encoding.Default;

            return Encrypt(encoding.GetBytes(key), encoding.GetBytes(value));
        }

        private static string ByteToHashString(byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", string.Empty);

        /// <summary>
        /// HMACSHA512 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">Hash Stream</param>
        /// <returns>유효여부</returns>
        public static bool Verify(byte[] key, Stream stream)
        {
            if (stream.CanSeek) stream.Position = 0;

            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                // Create an array to hold the keyed hash value read from the file.
                byte[] storedHash = new byte[hmac.HashSize / 8];
                stream.Read(storedHash, 0, storedHash.Length);
                byte[] computedHash = hmac.ComputeHash(stream);

                for (int i = 0; i < storedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// HMACSHA512 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="bytes">Hash Byte Data</param>
        /// <returns>유효여부</returns>
        public static bool Verify(byte[] key, byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Verify(key, ms);
            }
        }

        /// <summary>
        /// HMACSHA512 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="bytes">Hash Byte Data</param>
        /// <returns>유효여부</returns>
        public static bool Verify(string key, byte[] bytes)
            => Verify(Encoding.Default.GetBytes(key), bytes);

        /// <summary>
        /// HMACSHA512 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="value">Hash value</param>
        /// <param name="encoding">encoding</param>
        /// <returns>유효여부</returns>
        public static bool Verify(byte[] key, string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(key));

            encoding = encoding ?? Encoding.Default;

            return Verify(key, encoding.GetBytes(value));
        }

        /// <summary>
        /// HMACSHA512 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="value">Hash value</param>
        /// <param name="encoding">encoding</param>
        /// <returns>유효여부</returns>
        public static bool Verify(string key, string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(key));

            encoding = encoding ?? Encoding.Default;

            return Verify(encoding.GetBytes(key), encoding.GetBytes(value));
        }
    }
}