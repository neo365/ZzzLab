using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZzzLab.Crypt
{
    public class HMACSHA256Crypt
    {
        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">원본</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptToBytes(byte[] key, Stream stream)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            if (stream == null || stream.Length == 0) throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek) stream.Position = 0;
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(stream);
            }
        }

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">원본</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(byte[] key, Stream stream)
            => ByteToHashString(EncryptToBytes(key, stream));

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">원본</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(string key, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return Encrypt(Encoding.Default.GetBytes(key), stream);
        }

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(byte[] key, byte[] bytes)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Encrypt(key, ms);
            }
        }

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(string key, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return Encrypt(Encoding.Default.GetBytes(key), bytes);
        }

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(byte[] key, string value, Encoding encoding = null)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            encoding = encoding ?? Encoding.Default;

            return Encrypt(key, encoding.GetBytes(value));
        }

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Encrypt(string key, string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            encoding = encoding ?? Encoding.Default;

            return Encrypt(encoding.GetBytes(key), encoding.GetBytes(value));
        }

        /// <summary>
        /// HMACSHA256Crypt Signning
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ByteToHashString(byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", string.Empty);

        /// <summary>
        /// HMACSHA256 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="stream">Hash Stream</param>
        /// <returns>유효여부</returns>
        public static bool Verify(byte[] key, Stream stream)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            if (stream == null || stream.Length == 0) throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek) stream.Position = 0;

            using (HMACSHA256 hmac = new HMACSHA256(key))
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
        /// HMACSHA256 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="value">Hash Byte Data</param>
        /// <returns>유효여부</returns>
        public static bool Verify(byte[] key, byte[] value)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            if (value == null || value.Length == 0) throw new ArgumentNullException(nameof(value));

            using (MemoryStream ms = new MemoryStream(value))
            {
                return Verify(key, ms);
            }
        }

        /// <summary>
        /// HMACSHA256 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="value">Hash Byte Data</param>
        /// <returns>유효여부</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Verify(string key, byte[] value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null || value.Length == 0) throw new ArgumentNullException(nameof(value));

            return Verify(Encoding.Default.GetBytes(key), value);
        }

        /// <summary>
        /// HMACSHA256 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="value">Hash value</param>
        /// <param name="encoding">encoding</param>
        /// <returns>유효여부</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Verify(byte[] key, string value, Encoding encoding = null)
        {
            if (key == null || key.Length == 0) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(key));

            encoding = encoding ?? Encoding.Default;

            return Verify(key, encoding.GetBytes(value));
        }

        /// <summary>
        /// HMACSHA256 Verify
        /// </summary>
        /// <param name="key">secret key</param>
        /// <param name="value">Hash value</param>
        /// <param name="encoding">encoding</param>
        /// <returns>유효여부</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Verify(string key, string value, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            encoding = encoding ?? Encoding.Default;

            return Verify(encoding.GetBytes(key), encoding.GetBytes(value));
        }
    }
}