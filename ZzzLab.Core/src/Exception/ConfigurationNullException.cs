namespace System
{
    /// <summary>
    /// Initializes a new instance of the System.ConfigurationNullException class.
    /// </summary>
    public class ConfigurationNullException : InvalidArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the System.ConfigurationNullException class.
        /// </summary>
        public ConfigurationNullException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.ConfigurationNullException class.
        /// </summary>
        /// <param name="fieldName"></param>
        public ConfigurationNullException(string fieldName) : base(fieldName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.ConfigurationNullException class.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="message"></param>
        public ConfigurationNullException(string fieldName, string message) : base(fieldName, message)
        {
        }
    }
}