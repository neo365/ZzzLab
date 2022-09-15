namespace System
{
    /// <summary>
    /// System.NotFoundException class.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.NotFoundException class.
        /// </summary>
        public NotFoundException() : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.NotFoundException class.
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException(string message) : base(message)
        {
        }
    }
}