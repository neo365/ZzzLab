using System.IO;

namespace ZzzLab.Office
{
    public static class OfficeExtension
    {
        public static ImageType ToImageType(this string filePath)
        {
            string ext = Path.GetExtension(filePath);

            switch (ext.TrimStart('.').ToUpper())
            {
                case "EMF": return ImageType.EMF;
                case "WMF": return ImageType.WMF;
                case "PICT": return ImageType.PICT;
                case "JPG":
                case "JPEG":
                    return ImageType.JPG;

                case "PNG": return ImageType.PNG;
                case "DIB": return ImageType.DIB;
                case "GIF": return ImageType.GIF;

                case "TIF":
                case "TIFF":
                    return ImageType.TIFF;

                case "EPS": return ImageType.EPS;
                case "BMP": return ImageType.BMP;
                case "WPG": return ImageType.WPG;
                default: return ImageType.Unknown;
            }
        }
    }
}