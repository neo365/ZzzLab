using System;

namespace ZzzLab.Crypt
{
    public class BCryptHelper
    {
        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="passwd">Hash화할 값</param>
        /// <returns>Hash value</returns>
        public static string Encrypt(string passwd)
            => BCrypt.Net.BCrypt.HashPassword(passwd);

        /// <summary>
        /// 암호화된 문자열과 비밀번호가 일치하는 지 확인
        /// </summary>
        /// <param name="passwd"> 비밀번호</param>
        /// <param name="hashValue">Hash Value</param>
        /// <returns></returns>
        public static bool Verify(string passwd, string hashValue)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(passwd, hashValue);
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}