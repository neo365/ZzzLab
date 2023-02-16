using System;

namespace ZzzLab.IO
{
    public class FileFoundException : Exception
    {
        public string FilePath { private set; get; }
        public FileFoundException() : base(null)
        {
        }

        public FileFoundException(string message) : base(message)
        {
        }

        public FileFoundException(string message, string filePath) : base(message)
        {
            this.FilePath = filePath;
        }
    }
}