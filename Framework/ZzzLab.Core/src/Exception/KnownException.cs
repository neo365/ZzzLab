namespace System
{
    /// <summary>
    /// System.KnownException class.
    /// </summary>
    public class KnownException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.KnownException class.
        /// </summary>
        public KnownException() : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.KnownException class.
        /// </summary>
        /// <param name="message"></param>
        public KnownException(string message) : base(message)
        {
        }
    }
}