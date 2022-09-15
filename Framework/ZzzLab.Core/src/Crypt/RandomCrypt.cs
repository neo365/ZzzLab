using System;
using System.Collections.Generic;

namespace ZzzLab.Crypt
{
    public enum CharType
    {
        None = 0,
        AlpabetLower = 0x01, //1
        AlpabetUpper = 0x02, //2
        Number = 0x04, //4
        Special = 0x08, //8
        All = AlpabetLower | AlpabetUpper | Number | Special,
    }

    internal class RandomString
    {
        public string[] Strings { set; get; }

        private readonly Random nrandom = new Random();

        public static string Make(int lenth = 1)
        {
            RandomString ran = new RandomString();

            return ran.Get(lenth);
        }

        public string Get(int lenth = 1)
        {
            string result = string.Empty;

            for (int i = 0; i < lenth; i++)
            {
                result += Strings[nrandom.Next(Strings.Length)];
            }

            return result;
        }
    }

    public static class RandomCrypt
    {
        /// <summary>
        /// 지정된 최소/최대치 내에서 랜덤한 숫자 생성
        /// </summary>
        /// <param name="min">최소값</param>
        /// <param name="max">최대값</param>
        /// <returns>랜덤 숫자값</returns>
        public static int Create(int min, int max)
        {
            Random nrandom = new Random();
            return nrandom.Next(min, max);
        }

        private static string _LAST_RANDOM_VALUE = string.Empty;

        /// <summary>
        /// 랜덤 문자 생성
        /// </summary>
        /// <param name="chartype">문자타입</param>
        /// <param name="lenth">길이</param>
        /// <returns>랜덤문자값</returns>
        public static string Create(CharType chartype, int lenth)
        {
            string password = string.Empty;
            string[] AlpabetLowerArray = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z" };
            string[] AlpabetUpperArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z" };
            string[] NumberArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string[] SpecialArray = { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "{", "}", "[", "]", ":", ";", "\"", "'", "_", "-", "+", "=", "|", "\\", "~", "`", "<", ",", ">", ".", "?", "/" };

            // 빠르게 랜덤함수 호출하면 중복이 발생하는 문제로 마지막값이랑 다른게 나올때까지 돌리자.
            while (string.IsNullOrEmpty(password?.Trim()) || _LAST_RANDOM_VALUE.Equals(password))
            {
                int MaxValue = 0;
                List<RandomString> StringSet = new List<RandomString>();
                Random nrandom = new Random();

                password = string.Empty;

                if ((chartype & CharType.AlpabetLower) == CharType.AlpabetLower)
                {
                    RandomString item = new RandomString
                    {
                        Strings = AlpabetLowerArray
                    };

                    StringSet.Add(item);
                    MaxValue++;
                }

                if ((chartype & CharType.AlpabetUpper) == CharType.AlpabetUpper)
                {
                    RandomString item = new RandomString
                    {
                        Strings = AlpabetUpperArray
                    };

                    StringSet.Add(item);
                    MaxValue++;
                }

                if ((chartype & CharType.Number) == CharType.Number)
                {
                    RandomString item = new RandomString
                    {
                        Strings = NumberArray
                    };

                    StringSet.Add(item);
                    MaxValue++;
                }

                if ((chartype & CharType.Special) == CharType.Special)
                {
                    RandomString item = new RandomString
                    {
                        Strings = SpecialArray
                    };

                    StringSet.Add(item);
                    MaxValue++;
                }

                for (int i = 0; i < lenth; i++)
                {
                    password += StringSet[nrandom.Next(MaxValue)].Get(1);
                }
            }

            _LAST_RANDOM_VALUE = password;

            return password;
        }
    }
}