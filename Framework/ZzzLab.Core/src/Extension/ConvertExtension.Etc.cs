using System.Globalization;

namespace ZzzLab
{
    public static partial class ConvertExtension
    {
        public static string ToPascalCase(this string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";

            TextInfo ti = new CultureInfo("ko-KR", false).TextInfo;

            return ti.ToTitleCase(name.ToLower()).Replace("_", "");
        }
    }
}