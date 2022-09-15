using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZzzLab.Crypt
{
    /// <summary>
    /// AES 방식 암호화 모듈 (Use CBC)
    /// key와 iv 를 넣을 수도 있고 안 넣으면 내장된 key와 iv를 사용한다.
    /// </summary>
    public class AESCrypt //: ICrypt
    {
        private const string DEFAULT_KEY = "5209cd0198da49808102aca17a5bfc01";

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 16바이트 따서 사용
        /// </summary>
        /// <param name="text">암호화할 문자</param>
        /// <param name="key">키값 32~256자 사용</param>
        /// <param name="encoding">encoding</param>
        /// <returns>암호화된 데이터</returns>
        public static byte[] EncryptStringToBytes(string text, string key = AESCrypt.DEFAULT_KEY, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<byte>();
            if (string.IsNullOrEmpty(key)) key = AESCrypt.DEFAULT_KEY;

            encoding = encoding ?? Encoding.Default;

            byte[] inputBytes = encoding.GetBytes(key);
            byte[] keyBytes = new byte[32];
            byte[] ivBytes = new byte[16];
            Array.Copy(inputBytes, keyBytes, (inputBytes.Length > keyBytes.Length ? keyBytes.Length : inputBytes.Length));
            Array.Copy(keyBytes, ivBytes, (keyBytes.Length > ivBytes.Length ? ivBytes.Length : keyBytes.Length));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                            sw.Close();
                        }
                        cs.Close();
                    }

                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 16바이트 따서 사용
        /// </summary>
        /// <param name="text">암호화할 문자</param>
        /// <param name="key">키값 32~256자 사용</param>
        /// <returns>암호화된 문자(base64)</returns>
        public static string Encrypt(string text, string key = AESCrypt.DEFAULT_KEY)
            => Convert.ToBase64String(EncryptStringToBytes(text, key));

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 16바이트 따서 사용.
        /// URL에 사용할경우 일부 문자가 문제가 되므로 해당 문자를 다른문자로 치완한다.
        /// </summary>
        /// <param name="text">암호화할 문자</param>
        /// <param name="key">키값 32~256자 사용</param>
        /// <returns>암호화된 문자(base64)</returns>
        public static string EncryptUrlSafe(string text, string key = AESCrypt.DEFAULT_KEY)
            => Base64Crypt.EncryptUrlSafe(EncryptStringToBytes(text, key));

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 16바이트 따서 사용
        /// </summary>
        /// <param name="text">복호화할 문자</param>
        /// <param name="key">키값 32~256자 사용</param>
        /// <param name="encoding">encoding</param>
        /// <returns>복호화된 data</returns>
        public static byte[] DecryptBytesFromString(string text, string key = AESCrypt.DEFAULT_KEY, Encoding encoding = null)
        {
            if (text == null || text.Length <= 0) return Array.Empty<byte>();
            if (key == null || key.Length == 0) key = AESCrypt.DEFAULT_KEY;

            encoding = encoding ?? Encoding.Default;

            byte[] inputBytes = encoding.GetBytes(key);
            byte[] keyBytes = new byte[32];
            byte[] ivBytes = new byte[16];
            Array.Copy(inputBytes, keyBytes, (inputBytes.Length > keyBytes.Length ? keyBytes.Length : inputBytes.Length));
            Array.Copy(keyBytes, ivBytes, (keyBytes.Length > ivBytes.Length ? ivBytes.Length : keyBytes.Length));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] encryptedData = Convert.FromBase64String(text);
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedData, 0, encryptedData.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 16바이트 따서 사용
        /// </summary>
        /// <param name="text">복호화할 문자</param>
        /// <param name="key">키값 32~256자 사용</param>
        /// <param name="encoding">encoding</param>
        /// <returns>복호화된 문자</returns>
        public static string Decrypt(string text, string key = AESCrypt.DEFAULT_KEY, Encoding encoding = null)
            => (encoding ?? Encoding.Default).GetString(DecryptBytesFromString(text, key));

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 16바이트 따서 사용
        /// URL Safe를 이용한 경우 반드시 이 함수로 복원한다.
        /// </summary>
        /// <param name="text">복호화할 문자</param>
        /// <param name="key">키값 32~256자 사용</param>
        /// <param name="encoding">encoding</param>
        /// <returns>복호화된 문자</returns>
        public static string DecryptUrlSafe(string text, string key = AESCrypt.DEFAULT_KEY, Encoding encoding = null)
            => (encoding ?? Encoding.Default).GetString(DecryptBytesFromString(text.Replace(",", "=").Replace("-", "+").Replace("_", "/"), key));
    }
}