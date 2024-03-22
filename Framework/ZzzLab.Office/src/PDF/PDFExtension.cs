using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.Collections.Generic;
using System.IO;

namespace ZzzLab.Office.PDF
{
    public static class PDFExtension
    {
        public static byte[] MergePDF(this IEnumerable<byte[]> collection)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfDocument doc = new PdfDocument())
                {
                    foreach (byte[] item in collection)
                    {
                        using (MemoryStream stream = new MemoryStream(item))
                        {
                            using (PdfDocument sourceDoc = PdfReader.Open(stream, PdfDocumentOpenMode.Import))
                            {
                                doc.Version = sourceDoc.Version;
                                foreach (PdfPage page in sourceDoc.Pages)
                                {
                                    doc.AddPage(page);
                                }
                            }
                        }
                    }
                    doc.Save(ms);
                }

                return ms.ToArray();
            }
        }

        public static byte[] MergePDF(params byte[][] collection)
            => MergePDF((IEnumerable<byte[]>)collection);
    }
}