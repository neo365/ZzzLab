namespace System
{
    /// <summary>
    /// System.InitializeException class.
    /// </summary>
    public class InitializeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.InitializeException class.
        /// </summary>
        public InitializeException() : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InitializeException class.
        /// </summary>
        /// <param name="message">출력메세지</param>
        public InitializeException(string message) : base(message)
        {
        }
    }
}