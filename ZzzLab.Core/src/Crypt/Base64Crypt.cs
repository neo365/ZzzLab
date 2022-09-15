using System;
using System.Text;

namespace ZzzLab.Crypt
{
    /// <summary>
    /// Base64 변경
    /// </summary>
    public static class Base64Crypt
    {
        /// <summary>
        /// string을 base64로 변경
        /// </summary>
        /// <param name="s">변경할 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>base64 문자</returns>
        public static string Encrypt(string s, Encoding encoding = null)
            => Convert.ToBase64String((encoding ?? Encoding.Default).GetBytes(s ?? string.Empty));

        /// <summary>
        /// byte array를 base64로 변경
        /// </summary>
        /// <param name="bytes">변경할 byte array</param>
        /// <returns>base64 문자</returns>
        public static string Encrypt(byte[] bytes)
            => Convert.ToBase64String(bytes);

        /// <summary>
        /// string을 base64로 변경. URL에 사용할경우 일부 문자가 문제가 되므로 해당 문자를 다른문자로 치완한다.
        /// </summary>
        /// <param name="s">변경할 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>base64 문자</returns>
        public static string EncryptUrlSafe(string s, Encoding encoding = null)
            => Encrypt(s, encoding).Replace("=", ",").Replace("+", "-").Replace("_", "/");

        /// <summary>
        /// 데이터를 base64로 변경. URL에 사용할경우 일부 문자가 문제가 되므로 해당 문자를 다른문자로 치완한다.
        /// </summary>
        /// <param name="bytes">변경할 데이터</param>
        /// <returns>base64 문자</returns>
        public static string EncryptUrlSafe(byte[] bytes)
            => Encrypt(bytes).Replace("=", ",").Replace("+", "-").Replace("/", "_");

        /// <summary>
        /// string을 base64로 변경.
        /// </summary>
        /// <param name="s">변경할 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>base64 문자</returns>
        public static string ToBase64(this string s, Encoding encoding = null)
            => Encrypt(s, (encoding ?? Encoding.Default));

        /// <summary>
        /// string을 base64로 변경. URL에 사용할경우 일부 문자가 문제가 되므로 해당 문자를 다른문자로 치완한다.
        /// </summary>
        /// <param name="s">변경할 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>base64 문자</returns>
        public static string ToBase64UrlSafe(this string s, Encoding encoding = null)
            => EncryptUrlSafe(s, (encoding ?? Encoding.Default));

        /// <summary>
        /// 데이터를 base64로 변경. URL에 사용할경우 일부 문자가 문제가 되므로 해당 문자를 다른문자로 치완한다.
        /// </summary>
        /// <param name="bytes">변경할 데이터</param>
        /// <returns>base64 문자</returns>
        public static string ToBase64UrlSafe(this byte[] bytes)
            => EncryptUrlSafe(bytes);

        /// <summary>
        /// base64를 string으로 복원.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>복원된 string</returns>
        public static string Decrypt(string s, Encoding encoding = null)
            => (encoding ?? Encoding.Default).GetString(Convert.FromBase64String(s));

        /// <summary>
        /// base64를 데이터로 복원
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <returns>복원된 데이터</returns>
        public static byte[] Decrypt(string s)
            => Convert.FromBase64String(s);

        /// <summary>
        /// base64를 string으로 복원. UrlSafe로 base64를 만든경우 반드시 이함수로 복원할 것.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>복원된 string</returns>
        public static string DecryptUrlSafe(string s, Encoding encoding = null)
            => Decrypt(s.Replace(",", "=").Replace("-", "+").Replace("_", "/"), encoding);

        /// <summary>
        /// base64를 데이터로 복원. UrlSafe로 base64를 만든경우 반드시 이함수로 복원할 것.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <returns>복원된 데이터</returns>
        public static byte[] DecryptUrlSafe(string s)
            => Decrypt(s.Replace(",", "=").Replace("-", "+").Replace("_", "/"));

        /// <summary>
        /// base64를 string으로 복원.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <returns>복원된 string</returns>
        public static string FromBase64(this string s)
            => Decrypt(s, Encoding.Default);

        /// <summary>
        /// base64를 string으로 복원.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>복원된 string</returns>
        public static string FromBase64(this string s, Encoding encoding)
            => Decrypt(s, encoding);

        /// <summary>
        /// base64를 string으로 복원. UrlSafe로 base64를 만든경우 반드시 이함수로 복원할 것.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <returns>복원된 string</returns>
        public static string FromBase64UrlSafe(this string s)
            => DecryptUrlSafe(s, Encoding.Default);

        /// <summary>
        /// base64를 string으로 복원. UrlSafe로 base64를 만든경우 반드시 이함수로 복원할 것.
        /// </summary>
        /// <param name="s">base64 string</param>
        /// <param name="encoding">encoding 지정</param>
        /// <returns>복원된 string</returns>
        public static string FromBase64UrlSafe(this string s, Encoding encoding)
            => DecryptUrlSafe(s, encoding);
    }
}