namespace ZzzLab.Office
{
    public enum ImageType : int
    {
        //
        // 요약:
        //     Allow accessing the Initial value.
        Unknown = -1,

        None = 0,
        EMF = 2,
        WMF = 3,
        PICT = 4,
        JPG = 5,
        PNG = 6,
        DIB = 7,
        GIF = 8,
        TIFF = 9,
        EPS = 10,
        BMP = 11,
        WPG = 12
    }

    public enum Scale
    {
        Low = 1,
        High = 2,
        VeryHigh = 3
    }

    public enum CompressionLevel : long
    {
        High = 25L,
        Medium = 50L,
        Low = 90L,
        None = 100L
    }
}