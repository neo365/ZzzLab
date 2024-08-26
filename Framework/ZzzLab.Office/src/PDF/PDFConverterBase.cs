using System;
using System.Collections;
using System.IO;

namespace ZzzLab.Office.PDF
{
    public abstract class PDFConverterBase : IPdfConverter
    {
        public virtual string InputPath { get; protected set; }

        public virtual ConvType InputFileType { get; protected set; }

        public virtual Hashtable Options { get; set; } = new Hashtable();

        protected PDFConverterBase(string inputPath, Hashtable options)
        {
            if (string.IsNullOrWhiteSpace(inputPath)) throw new ArgumentNullException(nameof(inputPath));

            if (string.IsNullOrWhiteSpace(Path.GetDirectoryName(inputPath)))
            {
                inputPath = Path.Combine(Environment.CurrentDirectory, inputPath);
            }

            inputPath = Path.GetFullPath(inputPath);
            if (File.Exists(inputPath) == false) throw new FileNotFoundException();

            InputPath = inputPath;
            if (options != null) Options = options;

            Initialize();
        }

        protected abstract void Initialize();

        public abstract string ToPdf(string ouputPath = null);

        public virtual bool TryToPdf(ref string outputPath, out string message)
        {
            message = null;
            try
            {
                outputPath = ToPdf(outputPath);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return false;
        }
    }
}