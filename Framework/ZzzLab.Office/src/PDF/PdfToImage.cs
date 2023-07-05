using iTextSharp.text;
using iTextSharp.text.pdf;
using PDFiumSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ZzzLab.Office.PDF
{
    public static class PdfToImage
    {
        #region Public Methods

        /// <summary>
        /// Return a System.Drawing.Image collection with all the pdf pages
        /// </summary>
        /// <param name="bytes">contents of the file in a byte array</param>
        /// <param name="imageType">image Type</param>
        /// <param name="scale">Image resolution. Higher resolution generates bigger Image objects </param>
        /// /// <param name="pagenumbers"></param>
        /// <returns></returns>
        public static IEnumerable<PDFImage> ToImages(byte[] bytes, ImageType imageType = ImageType.PNG, Scale scale = Scale.High, IEnumerable<int> pagenumbers = null)
        {
            ImageFormat format;
            switch (imageType)
            {
                case ImageType.JPG: format = ImageFormat.Jpeg; break;
                case ImageType.PNG: format = ImageFormat.Png; break;
                case ImageType.BMP: format = ImageFormat.Bmp; break;
                case ImageType.GIF: format = ImageFormat.Gif; break;
                default: throw new NotSupportedException("Not Support file Format");
            }

            PdfReader.AllowOpenWithFullPermissions = true;
            PdfReader reader = new PdfReader(bytes);
            List<PDFImage> list = new List<PDFImage>();
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                if ((pagenumbers == null || pagenumbers.Any() == false || (pagenumbers.Any() && pagenumbers.Contains(i))))
                {
                    PDFImage img = new PDFImage
                    {
                        PageNumber = i,
                        Data = GetPdfImage(ExtractPdfPageStream(reader, i), format, scale),
                        Resolution = scale
                    };

                    list.Add(img);
                }
            }
            reader.Close();

            return list;
        }

        /// <summary>
        /// Return a System.Drawing.Image collection with all the pdf pages
        /// </summary>
        /// <param name="file">PDF file system source path</param>
        /// <param name="imageType">image Type</param>
        /// <param name="scale">Image resolution. Higher resolution generates bigger Image objects </param>
        /// <param name="pagenumbers"></param>
        /// <returns></returns>
        public static IEnumerable<PDFImage> ToImages(string file, ImageType imageType = ImageType.PNG, Scale scale = Scale.High, IEnumerable<int> pagenumbers = null)
        {
            if (File.Exists(file) == false) throw new FileNotFoundException();
            if (Path.GetExtension(file).EqualsIgnoreCase(".pdf") == false) throw new InvalidFileFormatException();

            byte[] bytes = File.ReadAllBytes(file);
            return ToImages(bytes, imageType, scale, pagenumbers);
        }

        /// <summary>
        /// Writes on outputFolder all the pdf pages as images with the resolution and format specified. Because the filename if not provided, all the generated files in outputFolder will start with "pdfpic"
        /// </summary>
        /// <param name="bytes">contents of the file in a byte array</param>
        /// <param name="outputFolder">Folder where the images will be generated</param>
        /// <param name="imageType">imageType</param>
        /// <param name="scale">Image resolution. Higher resolution generates bigger Image files</param>
        /// <param name="baseName"></param>
        /// <param name="pagenumbers"></param>
        public static int ToFile(byte[] bytes, string outputFolder, ImageType imageType = ImageType.PNG, Scale scale = Scale.High, string baseName = "pdfpic", IEnumerable<int> pagenumbers = null)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentNullException();

            //ImageFormat format = ImageFormat.Png;
            //switch (imageType)
            //{
            //    case ImageType.JPG: format = ImageFormat.Jpeg; break;
            //    case ImageType.PNG: format = ImageFormat.Png; break;
            //    case ImageType.BMP: format = ImageFormat.Bmp; break;
            //    case ImageType.GIF: format = ImageFormat.Gif; break;
            //    default: throw new NotSupportedException("Not Support file Format");
            //}

            if (imageType == ImageType.JPG
                || imageType == ImageType.PNG
                || imageType == ImageType.BMP
                || imageType == ImageType.GIF)
            {
                // Do Nothing
            }
            else
            {
                throw new NotSupportedException("Not Support file Format");
            }

            IEnumerable<PDFImage> images = ToImages(bytes, imageType, scale, pagenumbers);

            foreach (PDFImage image in images)
            {
                string outputPath = $"{outputFolder}\\{baseName}_{image.PageNumber}.{imageType.ToString().ToLower()}";

                //error will throw from here
                File.WriteAllBytes(outputPath, image.Data);
            }

            return images.Count();
        }

        /// <summary>
        /// Writes on outputFolder all the pdf pages as images with the resolution and format specified
        /// </summary>
        /// <param name="file">PDF file system source path</param>
        /// <param name="outputFolder">Folder where the images will be generated</param>
        /// <param name="imageType">imageType</param>
        /// <param name="scale">Image resolution. Higher resolution generates bigger Image files</param>
        /// <param name="baseName"></param>
        /// <param name="pagenumbers"></param>
        public static int ToFile(string file, string outputFolder, ImageType imageType = ImageType.PNG, Scale scale = Scale.High, string baseName = "", IEnumerable<int> pagenumbers = null)
        {
            if (File.Exists(file) == false) throw new FileNotFoundException();
            if (Path.GetExtension(file).EqualsIgnoreCase(".pdf") == false) throw new InvalidFileFormatException();
            if (string.IsNullOrWhiteSpace(baseName)) baseName = Path.GetFileNameWithoutExtension(file);

            byte[] bytes = File.ReadAllBytes(file);

            return ToFile(bytes, outputFolder, imageType, scale, baseName, pagenumbers);
        }

        #endregion Public Methods

        #region Private Methods

        //private static long GetCompression(CompressionLevel compression)
        //{
        //    switch (compression)
        //    {
        //        case CompressionLevel.High: return 25L;
        //        case CompressionLevel.Medium: return 50L;
        //        case CompressionLevel.Low: return 90L;
        //        case CompressionLevel.None: return 100L;
        //        default: return 100L;
        //    }
        //}

        //private static ImageCodecInfo GetEncoder(ImageFormat format)
        //{
        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        //    foreach (ImageCodecInfo codec in codecs)
        //    {
        //        if (codec.FormatID == format.Guid) return codec;
        //    }
        //    return null;
        //}

        private static byte[] GetPdfImage(byte[] pdf, ImageFormat imageFormat, Scale resolution = Scale.High)
        {
            using (var pdfDocument = new PDFiumSharp.PdfDocument(pdf))
            {
                var firstPage = pdfDocument.Pages[0]; //Only one page is expected here
                using (var pageBitmap = new PDFiumBitmap((int)firstPage.Size.Width * (int)resolution, (int)firstPage.Size.Height * (int)resolution, false))
                {
                    pageBitmap.Fill(new PDFiumSharp.Types.FPDF_COLOR(255, 255, 255)); //Lets fill the background with white RGB
                    firstPage.Render(pageBitmap);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(pageBitmap.AsBmpStream());

                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, imageFormat);

                        pdfDocument.Close();

                        return ms.ToArray();
                    }
                }
            }
        }

        private static byte[] ExtractPdfPageStream(PdfReader reader, int pagenumber)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document SourceDocument = new Document(reader.GetPageSizeWithRotation(pagenumber));
                PdfCopy PdfCopyProvider = new PdfCopy(SourceDocument, ms);
                SourceDocument.Open();
                PdfImportedPage ImportedPage = PdfCopyProvider.GetImportedPage(reader, pagenumber);
                PdfCopyProvider.AddPage(ImportedPage);
                SourceDocument.Close();
                return ms.ToArray();
            }
        }

        #endregion Private Methods
    }

    public class PDFImage
    {
        public int PageNumber { set; get; }
        public byte[] Data { set; get; }
        public ImageType Format { set; get; }
        public Scale Resolution { set; get; }
    }
}