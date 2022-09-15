namespace System
{
    /// <summary>
    /// Initializes a new instance of the System.DuplicateItemException class.
    /// </summary>
    public class DuplicateItemException : Exception
    {
        /// <summary>
        /// FieldName
        /// </summary>
        public string FieldName { set; get; }

        /// <summary>
        /// Initializes a new instance of the System.DuplicateItemException class.
        /// </summary>
        public DuplicateItemException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.DuplicateItemException class.
        /// </summary>
        /// <param name="fieldName"></param>
        public DuplicateItemException(string fieldName) : base($"Duplicate {fieldName} ")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the System.DuplicateItemException class.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="message"></param>
        public DuplicateItemException(string fieldName, string message) : base(message)
        {
            FieldName = fieldName;
        }
    }
}