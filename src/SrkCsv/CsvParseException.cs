
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class CsvParseException : Exception
    {
        private readonly string[] errors;

        public CsvParseException()
        {
        }

        public CsvParseException(string message)
            : base(message)
        {
        }

        public CsvParseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public CsvParseException(string message, IEnumerable<string> errors)
            : this(message)
        {
            this.errors = errors.ToArray();
        }

        protected CsvParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IList<string> Errors
        {
            get { return this.errors; }
        }
    }
}
