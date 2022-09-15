namespace System
{
    /// <summary>
    /// System.InvalidTypeException class.
    /// </summary>
    public class InvalidFileFormatException : Exception
    {
        public string FilePath { get; }

        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        public InvalidFileFormatException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        public InvalidFileFormatException(string filePath, string message) : base(message)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        /// <param name="filePath"></param>
        public InvalidFileFormatException(string filePath) : this(filePath, $"{filePath}은/는 잘못된 파일입니다.")
        {
        }
    }
}