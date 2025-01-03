using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZzzLab;

namespace System
{
    public static class StringExtension
    {
        /// <summary>
        ///  글자를 왼쪽기준으로 지정된 길이만큼 잘라낸다.
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="length">글자수</param>
        /// <returns>결과</returns>
        public static string Left(this string str, int length)
            => (length <= str.Length ? str.Substring(0, length) : str);

        /// <summary>
        ///  글자를 오른쪽기준으로 지정된 길이만큼 잘라낸다.
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="length">글자수</param>
        /// <returns>결과</returns>
        public static string Right(this string str, int length)
            => (length <= str.Length ? str.Substring(str.Length - length) : str);

        public static string ReplaceEscapeChar(this string str)
            => str.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");

        public static string Replace(this string str, char oldValue, string newValue)
            => str.Replace(oldValue.ToString(), newValue);

        public static string Replace(this string str, string oldValue, char newValue)
            => str.Replace(oldValue, newValue.ToString());

        public static string Replace(this string str, string[] oldCollection, string newValue)
        {
            foreach (string s in oldCollection)
            {
                str = str.Replace(s, newValue);
            }

            return str;
        }

        public static string Replace(this string str, char[] oldCollection, char newValue)
            => Replace(str, (IEnumerable<char>)oldCollection, newValue);

        public static string Replace(this string str, IEnumerable<char> oldCollection, char newValue)
        {
            foreach (char c in oldCollection)
            {
                str = str.Replace(c, newValue);
            }

            return str;
        }

        public static string ReplaceIgnoreCase(this string str, string oldValue, string newValue)
            => str.Replace(oldValue, newValue).Replace(oldValue.ToUpper(), newValue).Replace(oldValue.ToLower(), newValue);

        public static string ReplaceIgnoreCase(this string str, char oldValue, char newValue)
            => str.Replace(oldValue, newValue).Replace(oldValue.ToString().ToUpper(), newValue.ToString()).Replace(oldValue.ToString().ToLower(), newValue.ToString());

        public static string ReplaceIgnoreCase(this string str, char oldValue, string newValue)
            => str.ReplaceIgnoreCase(oldValue.ToString(), newValue);

        public static string ReplaceIgnoreCase(this string str, string oldValue, char newValue)
            => str.ReplaceIgnoreCase(oldValue, newValue.ToString());

        public static string ReplaceIgnoreCase(this string str, char[] oldCollection, char newValue)
            => ReplaceIgnoreCase(str, (IEnumerable<char>)oldCollection, newValue);

        public static string ReplaceIgnoreCase(this string str, IEnumerable<char> oldCollection, char newValue)
        {
            foreach (char c in oldCollection)
            {
                str = str.ReplaceIgnoreCase(c, newValue);
            }

            return str;
        }

        public static string ReplaceIgnoreCase(this string str, IEnumerable<char> oldCollection, string newValue)
        {
            foreach (char c in oldCollection)
            {
                str = str.ReplaceIgnoreCase(c, newValue);
            }

            return str;
        }

        public static string ReplaceIgnoreCase(this string str, IEnumerable<string> oldCollection, char newValue)
        {
            foreach (string c in oldCollection)
            {
                str = str.ReplaceIgnoreCase(c, newValue);
            }

            return str;
        }

        /// <summary>
        /// 문자열내에서 지정된 문자를 삭제한다
        /// </summary>
        /// <param name="str"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string Remove(this string str, params string[] collection)
            => Remove(str, (IEnumerable<string>)collection);

        /// <summary>
        /// 문자열내에서 지정된 문자를 삭제한다
        /// </summary>
        /// <param name="str"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string Remove(this string str, IEnumerable<string> collection)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            foreach (string value in collection)
            {
                if (string.IsNullOrEmpty(value)) continue;

                str = str.Replace(value, "");
            }
            return str;
        }

        /// <summary>
        /// 문자열내에서 지정된 문자를 삭제한다
        /// </summary>
        /// <param name="str"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string Remove(this string str, params char[] collection)
            => Remove(str, (IEnumerable<char>)collection);

        /// <summary>
        /// 문자열내에서 지정된 문자를 삭제한다
        /// </summary>
        /// <param name="str"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string Remove(this string str, IEnumerable<char> collection)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            foreach (char c in collection)
            {
                str = str.Replace(c, "");
            }
            return str;
        }

        public static string RemoveIgnoreCase(this string str, params string[] collection)
            => RemoveIgnoreCase(str, (IEnumerable<string>)collection);

        public static string RemoveIgnoreCase(this string str, IEnumerable<string> collection)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            foreach (string value in collection)
            {
                if (string.IsNullOrEmpty(value)) continue;
                str = ReplaceIgnoreCase(str, value, "");
            }
            return str;
        }

        /// <summary>
        /// 두개 이상의 연속된 공백을 하나의 공백으로 바꿔줌.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveDoubleSpace(this string str)
        {
            while (str.ContainsIgnoreCase("  "))
            {
                str = str.Replace("  ", " ");
            }

            return str.Trim();
        }

        public static string RemoveGroup(this string str, char start, char end)
        {
            string rs = str;
            while (rs.LastIndexOf(start) > 0)
            {
                string head = rs.Substring(0, rs.LastIndexOf(start));
                string tail = rs.Substring(rs.LastIndexOf(start));

                if (tail.IndexOf(end) > 0)
                {
                    if (tail.Length >= tail.IndexOf(end) + 1)
                    {
                        rs = string.Concat(head, tail.Substring(tail.IndexOf(end) + 1));
                    }
                    else rs = head;
                }
                else
                {
                    break;
                }
            }

            return rs;
        }

        public static string RemoveRex(this string s, string pattern, string appendHeader = "", string appendTail = "")
        {
            if (pattern == null || pattern.Length == 0) return s;

            Match m = Regex.Match(s, pattern);

            if (m.Length > 0)
            {
                string head = s.Substring(0, m.Index);
                string tail = s.Substring(m.Index + m.Value.Length);

                s = head + appendHeader + appendTail + tail;
            }

            return s;
        }

        public static string RemoveAllRex(this string s, string pattern, string appendHeader = "", string appendTail = "")
        {
            if (pattern == null || pattern.Length == 0) return s;

            while (true)
            {
                Match m = Regex.Match(s, pattern);

                if (m.Length > 0)
                {
                    string head = s.Substring(0, m.Index);
                    string tail = s.Substring(m.Index + m.Value.Length);

                    s = head + appendHeader + appendTail + tail;
                }
                else
                {
                    break;
                }
            }

            return s;
        }

        public static string RegexChange(this string s,
            string searchpattern,
            string replacePrttern,
            string newValue,
            string header = "",
            string tail = "")
        {
            Match match = Regex.Match(s, searchpattern);

            if (match.Length > 0)
            {
                string value = match.Value;

                if (string.IsNullOrEmpty(replacePrttern) == false && newValue != null)
                {
                    value = Regex.Replace(value, replacePrttern, newValue);
                }

                return s.Substring(0, match.Index)
                    + header
                    + value
                    + tail
                    + s.Substring(match.Index + match.Value.Length);
            }

            return s;
        }

        public static string RegexReplace(this string s, string newValue, params string[] patterns)
        {
            if (patterns == null || patterns.Length == 0) return s;

            foreach (string p in patterns)
            {
                if (string.IsNullOrEmpty(p) || string.IsNullOrEmpty(p.Trim())) continue;

                s = Regex.Replace(s, p, newValue);
            }

            return s;
        }

        public static string RegexAppend(this string s,
            string pattern,
            string header = "",
            string tail = "")
        {
            Match match = Regex.Match(s, pattern);

            if (match.Length > 0)
            {
                return s.Substring(0, match.Index)
                    + header
                    + match.Value
                    + tail
                    + s.Substring(match.Index + match.Value.Length);
            }

            return s;
        }

        public static string Concat(this IEnumerable<string> collection, string delimiter = ",")
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            //if (string.IsNullOrWhiteSpace(delimiter)) throw new ArgumentNullException(nameof(delimiter));

            string result = "";

            foreach (var value in collection)
            {
                if (string.IsNullOrWhiteSpace(value) == false) result += $"{delimiter}{value}";
            }

            return result.Trim().TrimStart(delimiter.ToCharArray()).Trim();
        }
    }
}