namespace System
{
    /// <summary>
    /// System.InvalidTypeException class.
    /// </summary>
    public class InvalidTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        public InvalidTypeException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        /// <param name="type"></param>
        public InvalidTypeException(Type type) : base($"{type.Name}은 잘못된 형식입니다.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public InvalidTypeException(Type type, string message) : base($"{type.Name}은 잘못된 형식입니다. {message}")
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidTypeException class.
        /// </summary>
        /// <param name="message"></param>
        public InvalidTypeException(string message) : base(message)
        {
        }
    }
}