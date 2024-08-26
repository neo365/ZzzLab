using System.Collections;

namespace ZzzLab.Office.PDF
{
    public interface IPdfConverter
    {
        string InputPath { get; }

        ConvType InputFileType { get; }

        Hashtable Options { get; set; }

        string ToPdf(string outputPath = null);

        bool TryToPdf(ref string outputPath, out string message);
    }

    public enum ConvType
    {
        Pdf,
        Excel,
        PowerPoint,
        Word,
        Visio,
        Publisher,
        Outlook,
        MSProject,
        AutoCad,
        HPGL,
        Image,
        Hwp,
        Xps
    }
}