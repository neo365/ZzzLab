using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace ZzzLab.Crawler
{
    public static class HtmlAgilityExtention
    {
        /// <summary>
        /// 지정된 attribute를 가져온다
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAttributes(this HtmlNode node, string attributeName)
        {
            if (node == null) return null;

            return node.Attributes.Contains(attributeName) ? node.Attributes[attributeName]?.Value.Trim().ParsingHtml() : null;
        }

        /// <summary>
        /// A tag의 링크 URL을 가져온다
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetAHref(this HtmlNode node)
        {
            if (node == null) return null;
            if (node.Name.EqualsIgnoreCase("a") == false) return null;

            return GetAttributes(node, "href");
        }

        /// <summary>
        /// img tag의 src를 가져온다.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetImgSrc(this HtmlNode node)
            => node.GetImgSrc(out _);

        /// <summary>
        /// img tag의 src를 가져온다.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetImgSrc(this HtmlNode node, out HtmlAttribute attr)
        {
            attr = null;
            if (node == null) return null;
            if (node.Name.EqualsIgnoreCase("img") == false) return null;

            string src = null; ;

            if (node.Attributes.Contains("src"))
            {
                attr = node.Attributes["src"];
                src = attr?.Value.Trim().ParsingHtml();
                if (string.IsNullOrWhiteSpace(src) == false) return src;
            }

            if (node.Attributes.Contains("data-cfsrc"))
            {
                attr = node.Attributes["data-cfsrc"];
                src = attr?.Value.Trim().ParsingHtml();
                if (string.IsNullOrWhiteSpace(src) == false) return src;
            }

            if (node.Attributes.Contains("data-src"))
            {
                attr = node.Attributes["data-src"];
                src = attr?.Value.Trim().ParsingHtml();
                if (string.IsNullOrWhiteSpace(src) == false) return src;
            }

            return src;
        }
        /// <summary>
        /// Inner Text를 가져올때 앞뒤의 줄바꿈, 공백, 탭을 삭제 한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string InnerTrim(this string value)
            => value?.Trim().TrimStart('\r', '\n', '\t').TrimEnd('\r', '\n', '\t').Trim();

        public static string ParsingHtml(this string value)
        => value.Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&nbsp;", " ")
                .Replace("<!--.*?-->", string.Empty, RegexOptions.Singleline)
                .Trim();

        public static string Replace(this string value, string pattern, string replacement, RegexOptions options)
            => Regex.Replace(value, pattern, replacement, options);
    }
}