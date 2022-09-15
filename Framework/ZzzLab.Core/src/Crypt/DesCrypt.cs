using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZzzLab.Crypt
{
    /// <summary>
    /// DES 방식 암호화 모듈 (Use CBC)
    /// key와 iv 를 넣을 수도 있고 안 넣으면 내장된 key와 iv를 사용한다.
    /// key와 iv는 8byte를 넘을 수 없다.
    /// </summary>
    public class DESCrypt
    {
        private const string DEFAULT_KEY = "5209cd01";

        #region DES암호화

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 8바이트 따서 사용
        /// </summary>
        /// <param name="text">암호화할 문자</param>
        /// <param name="key">키값 8byte 사용</param>
        /// <returns>암호화된 데이터</returns>
        public static byte[] EncryptStringToBytes(string text, string key = DESCrypt.DEFAULT_KEY)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<byte>();
            if (string.IsNullOrEmpty(key)) key = DESCrypt.DEFAULT_KEY;

            byte[] btSrc = Encoding.Default.GetBytes(text);

            using (DES desAlg = DES.Create())
            {
                byte[] btBytes = Encoding.ASCII.GetBytes(key);
                byte[] keyBytes = new byte[desAlg.KeySize / 8];
                Array.Copy(btBytes, keyBytes, (btBytes.Length > keyBytes.Length ? keyBytes.Length : btBytes.Length));

                byte[] ivBytes = new byte[desAlg.BlockSize / 8];
                Array.Copy(keyBytes, ivBytes, (keyBytes.Length > ivBytes.Length ? ivBytes.Length : keyBytes.Length));

                desAlg.Key = keyBytes;
                desAlg.IV = ivBytes;

                ICryptoTransform encryptor = desAlg.CreateEncryptor(keyBytes, ivBytes);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(btSrc, 0, btSrc.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 8바이트 따서 사용
        /// </summary>
        /// <param name="text">암호화할 문자</param>
        /// <param name="key">키값 8byte 사용</param>
        /// <returns>암호화된 문자</returns>
        public static string Encrypt(string text, string key = DESCrypt.DEFAULT_KEY)
            => Convert.ToBase64String(EncryptStringToBytes(text, key));

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 8바이트 따서 사용
        /// </summary>
        /// <param name="text">암호화할 문자</param>
        /// <param name="key">키값 8byte 사용</param>
        /// <returns>암호화된 문자</returns>
        public static string EncryptUrlSafe(string text, string key = DESCrypt.DEFAULT_KEY)
            => Base64Crypt.EncryptUrlSafe(EncryptStringToBytes(text, key));

        #endregion DES암호화

        #region 복호화

        /// <summary>
        /// CBC방식을 사용한다.
        /// IV(Initialization Vector)는 키에서 8바이트 따서 사용
        /// </summary>
        /// <param name="text">복호할 문자</param>
        /// <param name="key">키값 8byte 사용</param>
        /// <returns>복호화된 데이터</returns>
        public static byte[] DecryptBytesFromString(string text, string key = DESCrypt.DEFAULT_KEY)
        {
            if (string.IsNullOrEmpty(text)) return Array.Empty<byte>();
            if (string.IsNullOrEmpty(key)) key = DESCrypt.DEFAULT_KEY;

            byte[] encryptedData = Convert.FromBase64String(text);

            ////키 유효성 검사
            //byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            ////키가 8Byte가 아니면 예외발생
            //if (keyBytes.Length != 8) throw new InvalidDataException("Invalid key. Key length must be 8 byte.");

            using (DES desAlg = DES.Create())
            {
                byte[] btBytes = Encoding.ASCII.GetBytes(key);
                byte[] keyBytes = new byte[desAlg.KeySize / 8];
                Array.Copy(btBytes, keyBytes, (btBytes.Length > keyBytes.Length ? keyBytes.Length : btBytes.Length));

                byte[] ivBytes = new byte[desAlg.BlockSize / 8];
                Array.Copy(keyBytes, ivBytes, (keyBytes.Length > ivBytes.Length ? ivBytes.Length : keyBytes.Length));
                desAlg.Key = keyBytes;
                desAlg.IV = ivBytes;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream())
                {
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
        /// IV(Initialization Vector)는 키에서 8바이트 따서 사용
        /// </summary>
        /// <param name="text">복호할 문자</param>
        /// <param name="key">키값 8byte 사용</param>
        /// <returns>복호화된 문자</returns>
        public static string Decrypt(string text, string key = DESCrypt.DEFAULT_KEY)
            => Encoding.Default.GetString(DecryptBytesFromString(text, key));

        public static string DecryptUrlSafe(string text, string key = DESCrypt.DEFAULT_KEY)
            => Encoding.Default.GetString(DecryptBytesFromString(text.Replace(",", "=").Replace("-", "+").Replace("_", "/"), key));

        #endregion 복호화
    }
}