using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ZzzLab
{
    public static partial class ValidUtils
    {
        /// <summary>
        /// 이메일주소가 적합한지 판단.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>적합여부</returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            if (email.Contains('.', '@') == false) return false;

            return new EmailAddressAttribute().IsValid(email);
        }

        /// <summary>
        /// 이메일주소가 적합한지 판단.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>적합여부</returns>
        public static bool IsValidEmail(params string[] collection)
            => IsValidEmail((IEnumerable<string>)collection);

        /// <summary>
        /// 이메일주소가 적합한지 판단.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>적합여부</returns>
        public static bool IsValidEmail(this IEnumerable<string> collection)
        {
            if (collection == null || collection.Any() == false) return false;

            foreach (string email in collection)
            {
                if (IsValidEmail(email) == false) return false;
            }

            return true;
        }
    }
}