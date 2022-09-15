using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;

namespace ZzzLab.Crypt
{
    /// <summary>
    /// 확장된 기능을 가진 자바암호화 라이브러리
    /// </summary>
    public class BouncyCastleCrypt
    {
        private static PaddedBufferedBlockCipher cipher = cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()));

        public static string Encrypt(string text, string seed, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(seed)) throw new ArgumentNullException(nameof(seed));

            encoding = encoding ?? Encoding.Default;
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                byte[] inputBytes = encoding.GetBytes(seed);
                byte[] keyBytes = new byte[16];
                Array.Copy(inputBytes, keyBytes, (inputBytes.Length > keyBytes.Length ? keyBytes.Length : inputBytes.Length));

                cipher.Init(forEncryption: true, new KeyParameter(keyBytes));

                byte[] input = encoding.GetBytes(text);
                byte[] output = new byte[cipher.GetOutputSize(input.Length)];
                int num = cipher.ProcessBytes(input, 0, input.Length, output, 0);
                if (num > 0)
                {
                    byte[] array = Hex.Encode(output, 0, num);
                    stringBuilder.Append(encoding.GetString(array, 0, array.Length));
                }

                try
                {
                    num = cipher.DoFinal(output, 0);
                    if (num > 0)
                    {
                        byte[] array = Hex.Encode(output, 0, num);
                        stringBuilder.Append(encoding.GetString(array, 0, array.Length));
                    }
                }
                catch (CryptoException e)
                {
                    Logger.Warning(e);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return stringBuilder.ToString();
        }

        public static string Decrypt(string text, string seed, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(seed)) throw new ArgumentNullException(nameof(seed));

            encoding = encoding ?? Encoding.Default;
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                byte[] inputBytes = encoding.GetBytes(seed);
                byte[] keyBytes = new byte[16];
                Array.Copy(inputBytes, keyBytes, (inputBytes.Length > keyBytes.Length ? keyBytes.Length : inputBytes.Length));

                cipher.Init(forEncryption: false, new KeyParameter(keyBytes));

                byte[] input = Hex.Decode(text);
                byte[] output = new byte[cipher.GetOutputSize(input.Length)];

                int num = cipher.ProcessBytes(input, 0, input.Length, output, 0);
                if (num > 0) stringBuilder.Append(encoding.GetString(output, 0, num));

                try
                {
                    num = cipher.DoFinal(output, 0);
                    if (num > 0) stringBuilder.Append(encoding.GetString(output, 0, num));
                }
                catch (CryptoException e)
                {
                    Logger.Warning(e);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return stringBuilder.ToString();
        }
    }
}