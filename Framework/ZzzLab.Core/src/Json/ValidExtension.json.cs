using System;
using System.Text.Json;
using ZzzLab.Json;

namespace ZzzLab
{
    /// <summary>
    /// ValidExtension
    /// </summary>
    public static partial class ValidUtils
    {
        /// <summary>
        /// 문자열이 json이 맞는지 리턴
        /// </summary>
        /// <param name="jsonText">json 문자열</param>
        /// <returns>json 여부</returns>
        public static bool IsValidJsond(this string jsonText)
        {
            try
            {
                return JsonDocument.Parse(jsonText) != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// object가 json이 맞는지 리턴
        /// </summary>
        /// <param name="obj">json 문자열</param>
        /// <returns>json 여부</returns>
        public static bool IsValidJsond(this object obj)
        {
            if (obj == null) return false;
            if (obj is string jsonText) return jsonText.IsValidJsond();
            else
            {
                Type type = obj.GetType();
                if (type.IsClass)
                {
                    try
                    {
                        return (JsonConvert.SerializeObject(obj) != null);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}