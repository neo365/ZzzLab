using System;

namespace ZzzLab.IO
{
    // 사용샘플
    // Console.WriteLine(string.Format(new FileSizeFormatProvider(), "File size: {0:fs}", 100));
    // Console.WriteLine(string.Format(new FileSizeFormatProvider(), "File size: {0:fs}", 10000));
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType != null && formatType == typeof(ICustomFormatter)) return this;

            return null;
        }

        private const string fileSizeFormat = "fs";
        private const decimal OneKiloByte = 1024M;
        private const decimal OneMegaByte = OneKiloByte * 1024M;
        private const decimal OneGigaByte = OneMegaByte * 1024M;

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) throw new ArgumentNullException(nameof(arg));
            if (string.IsNullOrWhiteSpace(format) || format.StartsWith(fileSizeFormat, StringComparison.OrdinalIgnoreCase) == false)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            if (arg is string) return DefaultFormat(format, arg, formatProvider);

            decimal size;
            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "kB";
            }
            else suffix = " B";

            string precision = format.Substring(2);
            if (string.IsNullOrWhiteSpace(precision)) precision = "2";
            return string.Format("{0:N" + precision + "}{1}", size, suffix);
        }

        private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null) return string.Empty;
            if (arg is IFormattable formattableArg)
            {
                return formattableArg.ToString(format, formatProvider);
            }
            return string.Empty;
        }
    }
}