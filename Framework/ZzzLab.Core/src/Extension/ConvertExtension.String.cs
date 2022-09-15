using System;
using System.IO;
using System.Text;

namespace ZzzLab
{
    public static partial class ConvertExtension
    {
        /// <summary>
        ///  전각문자를 반각문자로 변경한다.
        /// </summary>
        /// <param name="s">전각문자</param>
        /// <returns>반각문자</returns>
        public static string Full2Half(this string s)
        {
            char[] ch = s.ToCharArray(0, s.Length);
            for (int i = 0; i < s.Length; ++i)
            {
                if (ch[i] > 0xff00 && ch[i] <= 0xff5e)
                    ch[i] -= (char)0xfee0;
                else if (ch[i] == 0x3000)
                    ch[i] = (char)0x20;
            }
            return (new string(ch));
        }

        // 반각 -> 전각

        /// <summary>
        /// 반각문자를 전각문자로 변경한다.
        /// </summary>
        /// <param name="s">반각문자</param>
        /// <returns>전각문자</returns>
        public static string Half2Full(this string s)
        {
            char[] ch = s.ToCharArray(0, s.Length);
            for (int i = 0; i < s.Length; ++i)
            {
                if (ch[i] > 0x21 && ch[i] <= 0x7e)
                    ch[i] += (char)0xfee0;
                else if (ch[i] == 0x20)
                    ch[i] = (char)0x3000;
            }
            return (new string(ch));
        }

        /// <summary>
        /// 문자열을 stream으로 변환한다.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Stream ToStream(this string str, Encoding encoding)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));
            return new MemoryStream((encoding ?? Encoding.Default).GetBytes(str)); ;
        }

        /// <summary>
        /// 문자열을 stream으로 변환한다.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="codepage"></param>
        /// <returns></returns>
        public static Stream ToStream(this string str, string codepage)
            => ToStream(str, Encoding.GetEncoding(codepage));

        /// <summary>
        /// 문자열을 stream으로 변환한다.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Stream ToStream(this string str)
            => ToStream(str, Encoding.Default);
    }
}