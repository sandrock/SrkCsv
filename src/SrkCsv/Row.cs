
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Row information and data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Row<T>
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();
        private int i;
        private IList<Cell<T>> cells = new List<Cell<T>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Row{T}"/> class.
        /// </summary>
        /// <param name="i">The i.</param>
        public Row(int i)
        {
            this.i = i;
            this.Errors = new List<string>();
            this.Warnings = new List<string>();
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        public int Index
        {
            get { return this.i; }
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public IList<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the warnings.
        /// </summary>
        public IList<string> Warnings { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        public T Target { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is header].
        /// </summary>
        public bool IsHeader { get; set; }

        public IList<Cell<T>> Cells
        {
            get { return this.cells; }
            set { this.cells = value; }
        }

        public void SetAttachment(T data)
        {
            this.data[typeof(T).FullName] = data;
        }

        public T GetAttachment()
        {
            var key = typeof(T).FullName;
            return this.data.ContainsKey(key) ? (T)this.data[key] : default(T);
        }

        public override string ToString()
        {
            if (this.cells != null)
                return "Row<" + typeof(T).Name + "> [" + this.i + "] " + string.Join(" ; ", this.cells);
            else
                return "Row<" + typeof(T).Name + "> [" + this.i + "] ";
        }

        internal virtual void SetTarget(object target)
        {
        }
    }
}
