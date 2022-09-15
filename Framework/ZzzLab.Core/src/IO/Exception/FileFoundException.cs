using System;

namespace ZzzLab.IO
{
    public class FileFoundException : Exception
    {
        public FileFoundException() : base(null)
        {
        }

        public FileFoundException(string message) : base(message)
        {
        }
    }
}