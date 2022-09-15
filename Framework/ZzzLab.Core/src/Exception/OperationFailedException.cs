namespace System
{
    /// <summary>
    /// System.OperationFailedException class.
    /// </summary>
    public class OperationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.OperationFailedException class.
        /// </summary>
        public OperationFailedException() : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.OperationFailedException class.
        /// </summary>
        /// <param name="message"></param>
        public OperationFailedException(string message) : base(message)
        {
        }
    }
}