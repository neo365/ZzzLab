namespace System
{
    /// <summary>
    ///  Initializes a new instance of the System.InvalidArgumentException class.
    /// </summary>
    public class InvalidArgumentException : Exception
    {
        /// <summary>
        ///  Initializes a new instance of the System.InvalidArgumentException class.
        /// </summary>
        public string FieldName { set; get; }

        /// <summary>
        /// Initializes a new instance of the System.InvalidArgumentException class.
        /// </summary>
        public InvalidArgumentException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidArgumentException class.
        /// </summary>
        /// <param name="FieldName"></param>
        public InvalidArgumentException(string FieldName) : this(FieldName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.InvalidArgumentException class.
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="message"></param>
        public InvalidArgumentException(string FieldName, string message) : base(message)
        {
            this.FieldName = FieldName;
        }
    }
}