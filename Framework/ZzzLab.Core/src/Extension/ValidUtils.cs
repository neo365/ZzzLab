using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZzzLab
{
    public static partial class ValidUtils
    {
        #region Null Check

        /// <summary>
        ///  Null 여부 판단.
        ///  DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Null 여부</returns>
        public static bool IsNull(object obj)
            => (obj is null || obj == DBNull.Value || obj.GetType() == typeof(DBNull));

        #endregion Null Check

        #region IsNullOrEmpty

        /// <summary>
        ///  null 또는 string.Empty 여부 판단.
        ///  DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Null or Empty 여부</returns>
        public static bool IsNullOrEmpty(object obj)
        {
            if (IsNull(obj)) return true;
            if (obj is string str) return string.IsNullOrEmpty(str);

            return false;
        }

        /// <summary>
        ///  null 또는 string.Empty 여부 판단.
        ///  DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        ///  하나라도 null 또는 string.Empty면 true를 리턴한다.
        /// </summary>
        /// <param name="collection"> object Array</param>
        /// <returns>Null or Empty 여부</returns>
        public static bool IsNullOrEmptyOr(params object[] collection)
            => IsNullOrEmptyOr(collection);

        /// <summary>
        ///  null 또는 string.Empty 여부 판단.
        ///  DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        ///  하나라도 null 또는 string.Empty면 true를 리턴한다.
        /// </summary>
        /// <param name="collection"> object collection</param>
        /// <returns>Null or Empty 여부</returns>
        public static bool IsNullOrEmptyOr(IEnumerable<object> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (object obj in collection)
            {
                if (IsNullOrEmpty(obj) == false) return false;
            }

            return true;
        }

        /// <summary>
        ///  null 또는 string.Empty 여부 판단.
        ///  DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        ///  하나라도 null 또는 string.Empty면 true를 리턴한다.
        /// </summary>
        /// <param name="collection"> object collection</param>
        /// <returns>Null or Empty 여부</returns>
        public static bool IsNullOrEmptyOr(params string[] collection)
            => IsNullOrEmptyOr((IEnumerable<string>)collection);

        /// <summary>
        ///  null 또는 string.Empty 여부 판단.
        ///  DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        ///  하나라도 null 또는 string.Empty면 true를 리턴한다.
        /// </summary>
        /// <param name="collection"> object collection</param>
        /// <returns>Null or Empty 여부</returns>
        public static bool IsNullOrEmptyOr(IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string str in collection)
            {
                if (string.IsNullOrEmpty(str) == false) return false;
            }

            return true;
        }

        #endregion IsNullOrEmpty

        #region IsNullOrWhiteSpace

        /// <summary>
        /// 전부 Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부 판단.
        /// DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        /// 하나라도 Null, string.Empty 또는 공백으로만 구성되어 있으면 true를 리턴한다.
        /// </summary>
        /// <param name="collection">string array</param>
        /// <returns>전부 Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부</returns>
        public static bool IsNullOrWhiteSpace(params string[] collection)
            => IsNullOrWhiteSpace((IEnumerable<string>)collection);

        /// <summary>
        /// 전부 Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부 판단.
        /// DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        /// 하나라도 Null, string.Empty 또는 공백으로만 구성되어 있으면 true를 리턴한다.
        /// </summary>
        /// <param name="collection">string collection</param>
        /// <returns>전부 Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부</returns>
        public static bool IsNullOrWhiteSpace(IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string str in collection)
            {
                if (string.IsNullOrWhiteSpace(str) == false) return false;
            }

            return true;
        }

        /// <summary>
        /// Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부 판단.
        /// DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        /// 하나라도 Null, string.Empty 또는 공백으로만 구성되어 있으면 true를 리턴한다.
        /// </summary>
        /// <param name="collection">string array</param>
        /// <returns>하나라도 Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부</returns>
        public static bool IsNullOrWhiteSpaceOr(params string[] collection)
            => IsNullOrWhiteSpaceOr((IEnumerable<string>)collection);

        /// <summary>
        /// Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부 판단.
        /// DB 리턴값이 null인경우는 DBNull 을 가지게 되므로 해당 여부도 체크한다.
        /// 하나라도 Null, string.Empty 또는 공백으로만 구성되어 있으면 true를 리턴한다.
        /// </summary>
        /// <param name="collection">string collection</param>
        /// <returns>하나라도 Null, string.Empty 또는 공백으로만 구성되어 있는 지 여부</returns>
        public static bool IsNullOrWhiteSpaceOr(IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string str in collection)
            {
                if (string.IsNullOrWhiteSpace(str)) return true;
            }

            return false;
        }

        #endregion IsNullOrWhiteSpace

        #region Equals

        /// <summary>
        /// 값이 하나라도 일치 하는지 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="values">비교대상 string array</param>
        /// <returns>값이 하나라도 일치 하는지 여부 판단</returns>
        public static bool EqualsOr(this string str, params string[] values)
            => EqualsOrIgnoreCase(str, (IEnumerable<string>)values);

        /// <summary>
        /// 값이 하나라도 일치 하는지 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string collection</param>
        /// <returns>값이 하나라도 일치 하는지 여부 판단</returns>
        public static bool EqualsOr(this string str, IEnumerable<string> collection)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (str.Equals(value)) return true;
            }
            return false;
        }

        /// <summary>
        /// 값이 일치 하는지 대소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="value">비교대상 string</param>
        /// <returns>값이 일치 하는지 여부 판단</returns>
        public static bool EqualsIgnoreCase(this string str, string value)
            => str.Equals(value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 값이 하나라도 일치 하는지 대소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string array</param>
        /// <returns>값이 하나라도 일치 하는지 여부 판단</returns>
        public static bool EqualsOrIgnoreCase(this string str, params string[] collection)
            => EqualsOrIgnoreCase(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 값이 하나라도 일치 하는지 대소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string collection</param>
        /// <returns>값이 하나라도 일치 하는지 여부 판단</returns>
        public static bool EqualsOrIgnoreCase(this string str, IEnumerable<string> collection)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (EqualsIgnoreCase(str, value)) return true;
            }

            return false;
        }

        #endregion Equals

        #region StartWith

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 시작하는지 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string array</param>
        /// <returns>값이 하나라도 비교대상 글자로 시작하는지 여부 판단</returns>
        public static bool StartsWithOr(this string str, params string[] collection)
            => StartsWithOr(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 시작하는지 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string collection</param>
        /// <returns>값이 하나라도 비교대상 글자로 시작하는지 여부 판단</returns>
        public static bool StartsWithOr(this string str, IEnumerable<string> collection)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (str.StartsWith(value)) return true;
            }
            return false;
        }

        /// <summary>
        /// 값이 비교대상 글자로 시작하는지 대/소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="value">비교대상 string</param>
        /// <returns>값이 비교대상 글자로 시작하는지 대/소문자를 무시하고 여부 판단</returns>
        public static bool StartsWithIgnoreCase(this string str, string value)
            => str.StartsWith(value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 시작하는지 대/소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string array</param>
        /// <returns>값이 하나라도 비교대상 글자로 시작하는지 대/소문자를 무시하고 여부 판단</returns>
        public static bool StartsWithIgnoreCaseOr(this string str, params string[] collection)
            => StartsWithIgnoreCaseOr(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 시작하는지 대/소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string collection</param>
        /// <returns>값이 하나라도 비교대상 글자로 시작하는지 대/소문자를 무시하고 여부 판단</returns>
        public static bool StartsWithIgnoreCaseOr(this string str, IEnumerable<string> collection)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (StartsWithIgnoreCase(str, value)) return true;
            }
            return false;
        }

        #endregion StartWith

        #region EndWith

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 끝나는지 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string array</param>
        /// <returns>값이 하나라도 비교대상 글자로 끝나는지 여부 판단</returns>
        public static bool EndsWithOr(this string str, params string[] collection)
            => EndsWithOr(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 끝나는지 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string collection</param>
        /// <returns>값이 하나라도 비교대상 글자로 끝나는지 여부 판단</returns>
        public static bool EndsWithOr(this string str, IEnumerable<string> collection)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (str.EndsWith(value) == true) return true;
            }
            return false;
        }

        /// <summary>
        /// 값이 비교대상 글자로 끝나는지 대/소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="value">비교대상 string </param>
        /// <returns>값이 비교대상 글자로 끝나는지 대/소문자를 무시하고 여부 판단</returns>
        public static bool EndsWithIgnoreCase(this string str, string value)
            => str.EndsWith(value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 끝나는지 대/소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string array</param>
        /// <returns>값이 하나라도 비교대상 글자로 끝나는지 대/소문자를 무시하고 여부 판단</returns>
        public static bool EndsWithIgnoreCaseOr(this string str, params string[] collection)
            => EndsWithIgnoreCaseOr(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 값이 하나라도 비교대상 글자로 끝나는지 대/소문자를 무시하고 여부 판단.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="collection">비교대상 string collection</param>
        /// <returns>값이 하나라도 비교대상 글자로 끝나는지 대/소문자를 무시하고 여부 판단</returns>
        public static bool EndsWithIgnoreCaseOr(this string str, IEnumerable<string> collection)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (EndsWithIgnoreCase(str, value) == true) return true;
            }
            return false;
        }

        #endregion EndWith

        #region Rex Support

        /// <summary>
        /// 정규표현식 패턴이 일치 하는 위치 반환.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="pattern">정규표현식 패턴</param>
        /// <returns>
        /// 0 미만: 패턴과 일치하는 글자 없음
        /// 0 이상 : 일치 하는 위치
        /// </returns>
        public static int IndexOfRex(this string str, string pattern)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException(nameof(pattern));

            Match match = Regex.Match(str, pattern);
            if (match.Length > 0) return match.Index;

            return -1;
        }

        /// <summary>
        /// 정규표현식 패턴이 일치 하는 마지막 위치 반환.
        /// </summary>
        /// <param name="str">판단 대상</param>
        /// <param name="pattern">정규표현식 패턴</param>
        /// <returns>
        /// 0 미만: 패턴과 일치하는 글자 없음
        /// 0 이상 : 일치 하는 위치
        /// </returns>
        public static bool EndsWithOrRex(this string str, string pattern)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException(nameof(pattern));

            Match match = Regex.Match(str, pattern);

            if (match.Length > 0)
            {
                if (string.IsNullOrEmpty(str.Substring(match.Index + match.Value.Length))) return true;
            }

            return false;
        }

        #endregion Rex Support

        #region 한글포함여부

        /// <summary>
        /// 한글여부 판단
        /// </summary>
        /// <param name="c">char</param>
        /// <returns>한글여부</returns>
        public static bool IsHangle(this char c)
            => ((c >= '\xAC00' && c <= '\xD7AF') || (c >= '\x3130' && c <= '\x318F'));

        /// <summary>
        /// 한글 포함여부 판단
        /// </summary>
        /// <param name="charArray">char array</param>
        /// <returns>포함여부</returns>
        public static bool IsHangle(this char[] charArray)
            => IsHangle((IEnumerable<char>)charArray);

        /// <summary>
        /// 한글 포함여부 판단
        /// </summary>
        /// <param name="collection">char collection</param>
        /// <returns>포함여부</returns>
        public static bool IsHangle(this IEnumerable<char> collection)
        {
            if (collection == null || collection.Any() == false) return false;
            foreach (char c in collection)
            {
                if (IsHangle(c)) return true;
            }

            return false;
        }

        /// <summary>
        /// 한글 포함여부 판단
        /// </summary>
        /// <param name="str">문자열</param>
        /// <returns>포함여부</returns>
        public static bool IsHangle(this string str)
            => (string.IsNullOrWhiteSpace(str) == false && IsHangle(str.ToCharArray()));

        #endregion 한글포함여부

        #region 숫자여부 판단

        /// <summary>
        /// 숫자여부 판단
        /// </summary>
        /// <param name="c">char</param>
        /// <returns>숫자여부</returns>
        public static bool IsDigit(this char c) => (c >= '0' && c <= '9');

        /// <summary>
        /// 숫자여부 판단
        /// </summary>
        /// <param name="charArray">char array</param>
        /// <returns>숫자여부</returns>
        public static bool IsDigit(this char[] charArray)
            => IsDigit((IEnumerable<char>)charArray);

        /// <summary>
        /// 숫자여부 판단
        /// </summary>
        /// <param name="collection">char collection</param>
        /// <returns>숫자여부</returns>
        public static bool IsDigit(this IEnumerable<char> collection)
        {
            foreach (char c in collection)
            {
                if (c.IsDigit() == false) return false;
            }

            return true;
        }

        /// <summary>
        /// 숫자여부 판단
        /// </summary>
        /// <param name="str">문자열</param>
        /// <returns>숫자여부</returns>
        public static bool IsDigit(this string str)
            => (string.IsNullOrWhiteSpace(str) == false && IsDigit(str.ToCharArray()));

        #endregion 숫자여부 판단

        #region int

        public static bool IsInt(this string s) => int.TryParse(s, out _);

        public static bool IsInt(this string[] collection)
            => IsInt((IEnumerable<string>)collection);

        public static bool IsInt(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsInt(value) == false) return false;
            }

            return true;
        }

        public static bool IsInt(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(DBNull)) return false;
            return IsInt(obj.ToString());
        }

        #endregion int

        #region uint

        public static bool IsUint(this string s) => uint.TryParse(s, out _);

        public static bool IsUint(this string[] collection)
            => IsUint((IEnumerable<string>)collection);

        public static bool IsUint(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsUint(value) == false) return false;
            }

            return true;
        }

        public static bool IsUint(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(DBNull)) return false;
            return obj.ToString().IsInt();
        }

        #endregion uint

        #region long

        public static bool IsLong(this string s) => long.TryParse(s, out _);

        public static bool IsLong(this string[] collection)
            => IsLong((IEnumerable<string>)collection);

        public static bool IsLong(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsLong(value) == false) return false;
            }

            return true;
        }

        public static bool IsLong(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(DBNull)) return false;
            return IsLong(obj.ToString());
        }

        #endregion long

        #region ulong

        public static bool IsUlong(this string s) => ulong.TryParse(s, out _);

        public static bool IsUlong(this string[] collection)
            => IsUlong((IEnumerable<string>)collection);

        public static bool IsUlong(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsUlong(value) == false) return false;
            }

            return true;
        }

        public static bool IsUlong(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(DBNull)) return false;

            return IsUlong(obj.ToString());
        }

        #endregion ulong

        #region double

        public static bool IsDouble(this string s) => double.TryParse(s, out _);

        public static bool IsDouble(this string[] collection)
            => IsDouble((IEnumerable<string>)collection);

        public static bool IsDouble(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsDouble(value) == false) return false;
            }

            return true;
        }

        public static bool IsDouble(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(DBNull)) return false;

            return IsDouble(obj.ToString());
        }

        #endregion double

        #region decimal

        public static bool IsDecimal(this string s) => decimal.TryParse(s, out _);

        public static bool IsDecimal(this string[] collection)
            => IsDecimal((IEnumerable<string>)collection);

        public static bool IsDecimal(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsDecimal(value) == false) return false;
            }

            return true;
        }

        public static bool IsDecimal(this object obj)
        {
            if (obj.GetType() == typeof(DBNull)) return false;
            return IsDecimal(obj.ToString());
        }

        #endregion decimal

        #region Float

        public static bool IsFloat(this string s) => float.TryParse(s, out _);

        public static bool IsFloat(this string[] collection)
            => IsFloat((IEnumerable<string>)collection);

        public static bool IsFloat(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsFloat(value) == false) return false;
            }

            return true;
        }

        public static bool IsFloat(this object obj)
        {
            if (obj == null || obj.GetType() == typeof(DBNull)) return false;
            return IsFloat(obj?.ToString());
        }

        #endregion Float

        #region boolean

        public static bool IsBoolean(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            if (bool.TryParse(str, out _)) return true;
            else if (str.ToUpper().EqualsOrIgnoreCase("Y", "N", "YES", "NO", "TRUE", "FALSE", "1", "0")) return true;

            return false;
        }

        public static bool IsBoolean(this string[] collection)
            => IsBoolean((IEnumerable<string>)collection);

        public static bool IsBoolean(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) throw new ArgumentNullException(nameof(collection));

            foreach (string value in collection)
            {
                if (string.IsNullOrWhiteSpace(value)) return false;
                if (IsBoolean(value) == false) return false;
            }

            return true;
        }

        public static bool IsBoolean(this object obj)
            => IsNull(obj) == false && IsBoolean(obj.ToString());

        #endregion boolean

        #region string

        /// <summary>
        /// string 여부판단.
        /// null 또는 DBNull일경우는 string 이 아니라고 판단함.
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>string 여부</returns>
        public static bool IsString(this object obj)
            => (IsNull(obj) == false && (obj is string));

        #endregion string

        #region DateTime

        public static bool IsDateTime(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            else
            {
                int count = 0;
                List<char> charlist = new List<char>();
                foreach (char c in str)
                {
                    char addChr = c;
                    if (c == ':')
                    {
                        count++;
                        if (count > 2) addChr = '.';
                        {
                        }
                    }

                    charlist.Add(addChr);
                }

                str = new string(charlist.ToArray());

                if (DateTime.TryParse(str, out _)) return true;
                else
                {
                    string[] formats =
                    {
                        "yyyyMMdd",
                        "yyyy-MM-dd"
                    };

                    return (DateTime.TryParseExact(str, formats, null, DateTimeStyles.None, out _));
                }
            }
        }

        public static bool IsDateTime(this object o)
        {
            if (o == null || o.GetType() == typeof(DBNull)) return false;

            return IsDateTime(o.ToString());
        }

        /// <summary>
        /// 날짜 형식이 맞는지 확인
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDate(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            else if (DateTime.TryParse(s, out _)) return true;
            else if (DateTime.TryParseExact(s, "yyyyMMdd", null, DateTimeStyles.AssumeLocal, out _)) return true;
            else if (DateTime.TryParseExact(s, "yyyy-MM-dd", null, DateTimeStyles.AssumeLocal, out _)) return true;
            else if (DateTime.TryParseExact(s, "yyMMdd", null, DateTimeStyles.AssumeLocal, out _)) return true;
            else if (DateTime.TryParseExact(s, "yy-MM-dd", null, DateTimeStyles.AssumeLocal, out _)) return true;

            return false;
        }

        #endregion DateTime

#if false
        public static void EncordCheck(this string str)
        {
            Encoding utf8 = Encoding.UTF8;
            EncodingInfo[] encods = Encoding.GetEncodings();

            foreach (EncodingInfo ec in encods)
            {
                Encoding enc = ec.GetEncoding();
                System.Diagnostics.Debug.WriteLine(string.Format("{0}({1}) : {2}", enc.EncodingName, enc.BodyName, utf8.GetString(enc.GetBytes(str))));
            }

            System.Diagnostics.Debug.WriteLine("------------------------------------------------");
        }
#endif

        /// <summary>
        /// 이 문자열 내에서 지정한 char가 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="value">찾을 문자</param>
        /// <returns>포함여부</returns>
        public static bool Contains(this string str, char value)
        {
            if (string.IsNullOrEmpty(str)) str = string.Empty;

            return str.Contains(value.ToString());
        }

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 모두 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool Contains(this string str, params string[] collection)
            => str.Contains((IEnumerable<string>)collection);

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 모두 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool Contains(this string str, IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (string s in collection)
            {
                if (str.Contains(s) == false) return false;
            }

            return true;
        }

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 모두 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool Contains(this string str, params char[] collection)
            => str.Contains((IEnumerable<char>)collection);

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 모두 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool Contains(this string str, IEnumerable<char> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (char c in collection)
            {
                if (str.Contains(c) == false) return false;
            }

            return true;
        }

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 하나라도 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">비교대상</param>
        /// <param name="collection">포함되어 있는지 확인할 문자 배열</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOr(this string str, params string[] collection)
            => ContainsOr(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 하나라도 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">비교대상</param>
        /// <param name="collection">포함되어 있는지 확인할 문자 배열</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOr(this string str, IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (string value in collection)
            {
                if (str.Contains(value)) return true;
            }

            return false;
        }

        /// <summary>
        /// 문자에 지정된 char들이 포함되어있는지 여부 리턴
        /// </summary>
        /// <param name="str">비교대상</param>
        /// <param name="collection">포함되어 있는지 확인할 char Array</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOr(this string str, params char[] collection)
            => ContainsOrIgnoreCase(str, (IEnumerable<char>)collection);

        /// <summary>
        /// 문자에 지정된 char들이 포함되어있는지 여부 리턴
        /// </summary>
        /// <param name="str">비교대상</param>
        /// <param name="collection">포함되어 있는지 확인할 char Array</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOr(this string str, IEnumerable<char> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (char value in collection)
            {
                if (Contains(str, value)) return true;
            }

            return false;
        }

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 대소문자 구분없이 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="value">찾을 문자</param>
        /// <returns>포함여부</returns>
        public static bool ContainsIgnoreCase(this string str, string value)
        {
            if (string.IsNullOrEmpty(str)) str = string.Empty;
            if (string.IsNullOrEmpty(value)) return false;

            return str.ToUpper().Contains(value.ToUpper());
        }

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 대소문자 구분없이 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="value">찾을 문자</param>
        /// <returns>포함여부</returns>
        public static bool ContainsIgnoreCase(this string str, char value)
            => ContainsIgnoreCase(str ?? string.Empty, value.ToString());

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 하나라도 대소문자 구분없이 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="collection">찾을 문자</param>
        /// <returns>포함여부</returns>

        public static bool ContainsOrIgnoreCase(this string str, params string[] collection)
            => ContainsOrIgnoreCase(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 하나라도 대소문자 구분없이 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="collection">찾을 문자</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOrIgnoreCase(this string str, IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (string value in collection)
            {
                if (ContainsIgnoreCase(str, value)) return true;
            }

            return false;
        }

        /// <summary>
        /// 문자에 지정된 char들이 대소문자 상관없이 포함되어있는지 여부 리턴
        /// </summary>
        /// <param name="str">비교대상</param>
        /// <param name="collection">포함되어 있는지 확인할 char Array</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOrIgnoreCase(this string str, params char[] collection)
            => ContainsOrIgnoreCase(str, (IEnumerable<char>)collection);

        /// <summary>
        /// 문자에 지정된 char들이 대소문자 상관없이 포함되어있는지 여부 리턴
        /// </summary>
        /// <param name="str">비교대상</param>
        /// <param name="collection">포함되어 있는지 확인할 char Array</param>
        /// <returns>포함여부</returns>
        public static bool ContainsOrIgnoreCase(this string str, IEnumerable<char> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (char value in collection)
            {
                if (ContainsIgnoreCase(str, value)) return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static bool IsMatch(this string str, params string[] patterns)
        {
            if (patterns != null && patterns.Length > 0)
            {
                foreach (string p in patterns)
                {
                    if (Regex.IsMatch(str, p) == false) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 이 문자열 내에서 지정한 문자가 하나라도 대소문자 구분없이 표시되는지를 나타내는 값을 반환합니다
        /// </summary>
        /// <param name="str"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static bool IsMatchOr(this string str, params string[] patterns)
        {
            if (patterns != null && patterns.Length > 0)
            {
                foreach (string p in patterns)
                {
                    if (Regex.IsMatch(str, p)) return true;
                }
            }

            return false;
        }
    }
}