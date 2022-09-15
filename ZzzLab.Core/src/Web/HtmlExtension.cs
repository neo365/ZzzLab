using System;
using System.Text.RegularExpressions;
using System.Web;

namespace ZzzLab.Web
{
    public static class HtmlExtension
    {
        public static string HtmlClearning(this string s)
        {
            return s.Remove("\t", "\r", "\n")
                    .HtmlToText()
                    .RemoveDoubleSpace().Trim();
        }

        public static string HtmlToText(this string s)
        {
            return s.Replace("&quot;", "\"") //따옴표
                    .Replace("&amp;", "&") //Ampersand
                    .Replace("&lt;", "<") //보다 작은
                    .Replace("&gt;", ">") //보다 큰
                    .Replace("&nbsp;", "") //Non-breaking space
                    .Replace("&iexcl;", "¡") //거꾸로된 느낌표
                    .Replace("&cent;", "￠") //센트 기호
                    .Replace("&pound;", "￡") //파운드
                    .Replace("&curren;", "¤") //현재 환율
                    .Replace("&yen;", "￥") //엔
                    .Replace("&brvbar;", "|") //끊어진 수직선
                    .Replace("&sect;", "§") //섹션 기호
                    .Replace("&uml;", "¨") //움라우트
                    .Replace("&copy;", "ⓒ") //저작권
                    .Replace("&ordf;", "ª") //Feminine ordinal
                    .Replace("&laquo;", "≪") //왼쪽 꺾인 괄호
                    .Replace("&not;", "￢") //부정
                    .Replace("&shy;", "") //Soft hyphen
                    .Replace("&reg;", "?") //등록상표
                    .Replace("&macr;", "&hibar;") //Macron accent
                    .Replace("&deg;", "°") //Degree sign
                    .Replace("&plusmn;", "±") //Plus or minus
                    .Replace("&sup2;", "²") //Superscript two
                    .Replace("&sup3;", "³") //Superscript three
                    .Replace("&acute;", "´") //Acute accent
                    .Replace("&micro;", "μ") //Micro sign (Mu)
                    .Replace("&para;", "¶") //문단기호
                    .Replace("&middot;", "·") //Middle dot
                    .Replace("&cedil;", "¸") //Cedilla
                    .Replace("&sup1;", "¹") //Superscript one
                    .Replace("&ordm;", "º") //Masculine ordinal
                    .Replace("&raquo;", "≫") //오른쪽 꺾인 괄호
                    .Replace("&frac14;", "¼") //4분의 1
                    .Replace("&frac12;", "½") //2분의 1
                    .Replace("&frac34;", "¾") //4분의 3
                    .Replace("&iquest;", "¿") //거꾸로된 물음표
                    .Replace("&Agrave;", "A") //Capital A, grave accent
                    .Replace("&Aacute;", "A") //Capital A, acute accent
                    .Replace("&Acirc;", "A") //Capital A, circumflex accent
                    .Replace("&Atilde;", "A") //Capital A, tilde
                    .Replace("&Auml;", "A") //Capital A, dieresis or umlaut mark
                    .Replace("&Aring;", "A") //Capital A, ring (Angstrom)
                    .Replace("&AElig;", "Æ") //Capital AE diphthong (ligature)
                    .Replace("&Ccedil;", "C") //Capital C, cedilla
                    .Replace("&Egrave;", "E") //Capital E, grave accent
                    .Replace("&Eacute;", "E") //Capital E, acute accent
                    .Replace("&Ecirc;", "E") //Capital E, circumflex accent
                    .Replace("&Euml;", "E") //Capital E, dieresis or umlaut mark
                    .Replace("&Igrave;", "I") //Capital I, grave accent
                    .Replace("&Iacute;", "I") //Capital I, acute accent
                    .Replace("&Icirc;", "I") //Capital I, circumflex accent
                    .Replace("&Iuml;", "I") //Capital I, dieresis or umlaut mark
                    .Replace("&ETH;", "Ð") //Capital Eth, Icelandic
                    .Replace("&Ntilde;", "N") //Capital N, tilde
                    .Replace("&Ograve;", "O") //Capital O, grave accent
                    .Replace("&Oacute;", "O") //Capital O, acute accent
                    .Replace("&Ocirc;", "O") //Capital O, circumflex accent
                    .Replace("&Otilde;", "O") //Capital O, tilde
                    .Replace("&Ouml;", "O") //Capital O, dieresis or umlaut mark
                    .Replace("&times;", "×") //Multiply sign
                    .Replace("&Oslash;", "Ø") //width="130"Capital O, slash
                    .Replace("&Ugrave;", "U") //Capital U, grave accent
                    .Replace("&Uacute;", "U") //Capital U, acute accent
                    .Replace("&Ucirc;", "U") //Capital U, circumflex accent
                    .Replace("&Uuml;", "U") //Capital U, dieresis or umlaut mark
                    .Replace("&Yacute;", "Y") //Capital Y, acute accent
                    .Replace("&THORN;", "Þ") //Capital Thorn, Icelandic
                    .Replace("&szlig;", "ß") //Small sharp s, German (sz ligature)
                    .Replace("&agrave;", "a") //Small a, grave accent
                    .Replace("&aacute;", "a") //Small a, acute accent
                    .Replace("&acirc;", "a") //Small a, circumflex accent
                    .Replace("&atilde;", "a") //Small a, tilde
                    .Replace("&auml;", "a") //Small a, dieresis or umlaut mark
                    .Replace("&aring;", "a") //Small a, ring
                    .Replace("&aelig;", "æ") //Small ae diphthong (ligature)
                    .Replace("&ccedil;", "c") //Small c, cedilla
                    .Replace("&egrave;", "e") //Small e, grave accent
                    .Replace("&eacute;", "e") //Small e, acute accent
                    .Replace("&ecirc;", "e") //Small e, circumflex accent
                    .Replace("&euml;", "e") //Small e, dieresis or umlaut mark
                    .Replace("&igrave;", "i") //Small i, grave accent
                    .Replace("&iacute;", "i") //Small i, acute accent
                    .Replace("&icirc;", "i") //Small i, circumflex accent
                    .Replace("&iuml;", "i") //Small i, dieresis or umlaut mark
                    .Replace("&eth;", "ð") //Small eth, Icelandic
                    .Replace("&ntilde;", "n") //Small n, tilde
                    .Replace("&ograve;", "o") //Small o, grave accent
                    .Replace("&oacute;", "o") //Small o, acute accent
                    .Replace("&ocirc;", "o") //Small o, circumflex accent
                    .Replace("&otilde;", "o") //Small o, tilde
                    .Replace("&ouml;", "o") //Small o, dieresis or umlaut mark
                    .Replace("&divide;", "÷") //Division sign
                    .Replace("&oslash;", "ø") //Small o, slash
                    .Replace("&ugrave;", "u") //Small u, grave accent
                    .Replace("&uacute;", "u") //Small u, acute accent
                    .Replace("&ucirc;", "u") //Small u, circumflex accent
                    .Replace("&uuml;", "u") //Small u, dieresis or umlaut mark
                    .Replace("&yacute;", "y") //Small y, acute accent
                    .Replace("&thorn;", "þ") //Small thorn, Icelandic
                    .Replace("&yuml;", "y") //Small y, dieresis or umlaut mark
                                            //----------
                    .Replace("&quot", "\"") //따옴표
                    .Replace("&amp", "&") //Ampersand
                    .Replace("&lt", "<") //보다 작은
                    .Replace("&gt", ">") //보다 큰
                    .Replace("&nbsp", "") //Non-breaking space
                    .Replace("&iexcl", "¡") //거꾸로된 느낌표
                    .Replace("&cent", "￠") //센트 기호
                    .Replace("&pound", "￡") //파운드
                    .Replace("&curren", "¤") //현재 환율
                    .Replace("&yen", "￥") //엔
                    .Replace("&brvbar", "|") //끊어진 수직선
                    .Replace("&sect", "§") //섹션 기호
                    .Replace("&uml", "¨") //움라우트
                    .Replace("&copy", "ⓒ") //저작권
                    .Replace("&ordf", "ª") //Feminine ordinal
                    .Replace("&laquo", "≪") //왼쪽 꺾인 괄호
                    .Replace("&not", "￢") //부정
                    .Replace("&shy", "") //Soft hyphen
                    .Replace("&reg", "?") //등록상표
                    .Replace("&macr", "&hibar;") //Macron accent
                    .Replace("&deg", "°") //Degree sign
                    .Replace("&plusmn", "±") //Plus or minus
                    .Replace("&sup2", "²") //Superscript two
                    .Replace("&sup3", "³") //Superscript three
                    .Replace("&acute", "´") //Acute accent
                    .Replace("&micro", "μ") //Micro sign (Mu)
                    .Replace("&para", "¶") //문단기호
                    .Replace("&middot", "·") //Middle dot
                    .Replace("&cedil", "¸") //Cedilla
                    .Replace("&sup1", "¹") //Superscript one
                    .Replace("&ordm", "º") //Masculine ordinal
                    .Replace("&raquo", "≫") //오른쪽 꺾인 괄호
                    .Replace("&frac14", "¼") //4분의 1
                    .Replace("&frac12", "½") //2분의 1
                    .Replace("&frac34", "¾") //4분의 3
                    .Replace("&iquest", "¿") //거꾸로된 물음표
                    .Replace("&Agrave", "A") //Capital A, grave accent
                    .Replace("&Aacute", "A") //Capital A, acute accent
                    .Replace("&Acirc", "A") //Capital A, circumflex accent
                    .Replace("&Atilde", "A") //Capital A, tilde
                    .Replace("&Auml", "A") //Capital A, dieresis or umlaut mark
                    .Replace("&Aring", "A") //Capital A, ring (Angstrom)
                    .Replace("&AElig", "Æ") //Capital AE diphthong (ligature)
                    .Replace("&Ccedil", "C") //Capital C, cedilla
                    .Replace("&Egrave", "E") //Capital E, grave accent
                    .Replace("&Eacute", "E") //Capital E, acute accent
                    .Replace("&Ecirc", "E") //Capital E, circumflex accent
                    .Replace("&Euml", "E") //Capital E, dieresis or umlaut mark
                    .Replace("&Igrave", "I") //Capital I, grave accent
                    .Replace("&Iacute", "I") //Capital I, acute accent
                    .Replace("&Icirc", "I") //Capital I, circumflex accent
                    .Replace("&Iuml", "I") //Capital I, dieresis or umlaut mark
                    .Replace("&ETH", "Ð") //Capital Eth, Icelandic
                    .Replace("&Ntilde", "N") //Capital N, tilde
                    .Replace("&Ograve", "O") //Capital O, grave accent
                    .Replace("&Oacute", "O") //Capital O, acute accent
                    .Replace("&Ocirc", "O") //Capital O, circumflex accent
                    .Replace("&Otilde", "O") //Capital O, tilde
                    .Replace("&Ouml", "O") //Capital O, dieresis or umlaut mark
                    .Replace("&times", "×") //Multiply sign
                    .Replace("&Oslash", "Ø") //width="130"Capital O, slash
                    .Replace("&Ugrave", "U") //Capital U, grave accent
                    .Replace("&Uacute", "U") //Capital U, acute accent
                    .Replace("&Ucirc", "U") //Capital U, circumflex accent
                    .Replace("&Uuml", "U") //Capital U, dieresis or umlaut mark
                    .Replace("&Yacute", "Y") //Capital Y, acute accent
                    .Replace("&THORN", "Þ") //Capital Thorn, Icelandic
                    .Replace("&szlig", "ß") //Small sharp s, German (sz ligature)
                    .Replace("&agrave", "a") //Small a, grave accent
                    .Replace("&aacute", "a") //Small a, acute accent
                    .Replace("&acirc", "a") //Small a, circumflex accent
                    .Replace("&atilde", "a") //Small a, tilde
                    .Replace("&auml", "a") //Small a, dieresis or umlaut mark
                    .Replace("&aring", "a") //Small a, ring
                    .Replace("&aelig", "æ") //Small ae diphthong (ligature)
                    .Replace("&ccedil", "c") //Small c, cedilla
                    .Replace("&egrave", "e") //Small e, grave accent
                    .Replace("&eacute", "e") //Small e, acute accent
                    .Replace("&ecirc", "e") //Small e, circumflex accent
                    .Replace("&euml", "e") //Small e, dieresis or umlaut mark
                    .Replace("&igrave", "i") //Small i, grave accent
                    .Replace("&iacute", "i") //Small i, acute accent
                    .Replace("&icirc", "i") //Small i, circumflex accent
                    .Replace("&iuml", "i") //Small i, dieresis or umlaut mark
                    .Replace("&eth", "ð") //Small eth, Icelandic
                    .Replace("&ntilde", "n") //Small n, tilde
                    .Replace("&ograve", "o") //Small o, grave accent
                    .Replace("&oacute", "o") //Small o, acute accent
                    .Replace("&ocirc", "o") //Small o, circumflex accent
                    .Replace("&otilde", "o") //Small o, tilde
                    .Replace("&ouml", "o") //Small o, dieresis or umlaut mark
                    .Replace("&divide", "÷") //Division sign
                    .Replace("&oslash", "ø") //Small o, slash
                    .Replace("&ugrave", "u") //Small u, grave accent
                    .Replace("&uacute", "u") //Small u, acute accent
                    .Replace("&ucirc", "u") //Small u, circumflex accent
                    .Replace("&uuml", "u") //Small u, dieresis or umlaut mark
                    .Replace("&yacute", "y") //Small y, acute accent
                    .Replace("&thorn", "þ") //Small thorn, Icelandic
                    .Replace("&yuml", "y") //Small y, dieresis or umlaut mark
                    .Trim();
        }

        public static string HtmlToString(this string s)
        {
            return Regex.Replace(HttpUtility.HtmlDecode(s), "<!--.*-->", "");
        }
    }
}