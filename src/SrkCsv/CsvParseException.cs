
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception that occur during parsing operations.
    /// </summary>
    [Serializable]
    public class CsvParseException : Exception
    {
        private readonly string[] errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        public CsvParseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CsvParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public CsvParseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        public CsvParseException(string message, IEnumerable<string> errors)
            : this(message)
        {
            this.errors = errors.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected CsvParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IList<string> Errors
        {
            get { return this.errors; }
        }
    }
}
