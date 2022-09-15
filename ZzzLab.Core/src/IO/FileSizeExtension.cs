using System.IO;

namespace ZzzLab.IO
{
    public static class FileSizeExtension
    {
        public static string ToFileSize(this long l)
        {
            return string.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }

        public static string ToFileSize(this int i)
        {
            return string.Format(new FileSizeFormatProvider(), "{0:fs}", i);
        }

        public static string ToFileSize(this decimal d)
        {
            return string.Format(new FileSizeFormatProvider(), "{0:fs}", d);
        }

        public static string ToFileSize(this FileInfo f)
        {
            long fileSize = (f.Exists ? f.Length : 0L);
            return string.Format(new FileSizeFormatProvider(), "{0:fs}", fileSize);
        }

        public static long GetFileLength(this string f)
        {
            if (string.IsNullOrEmpty(f?.Trim())) return 0L;

            FileInfo fi = new FileInfo(f);
            if (fi.Exists) return fi.Length;
            return -1L;
        }
    }
}