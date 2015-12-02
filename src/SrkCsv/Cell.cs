
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Cell information and data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Cell<T>
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell{T}"/> class.
        /// </summary>
        public Cell()
        {
        }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        public Row<T> Row { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        public Column<T> Column { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        public T Target { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return "Cell<" + typeof(T).Name + "> " + this.Value;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.Row = null;
                    this.Column = null;
                }

                this.isDisposed = true;
            }
        }

        internal object GetTarget()
        {
            return null;
        }
    }
}
