namespace System
{
    /// <summary>
    /// Initializes a new instance of the System.DuplicateInitializeException class.
    /// </summary>
    public class DuplicateInitializeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.DuplicateInitializeException class.
        /// </summary>
        public DuplicateInitializeException() : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.DuplicateInitializeException class.
        /// </summary>
        /// <param name="message"></param>
        public DuplicateInitializeException(string message) : base(message)
        {
        }
    }
}