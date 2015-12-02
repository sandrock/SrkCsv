
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Column information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Column<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Column{T}"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="col">The col.</param>
        public Column(int index, string col)
        {
            this.Index = index;
            this.Name = col;
        }

        /// <summary>
        /// Gets or sets the column index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        internal Predicate<Cell<T>> Transform { get; set; }

        public override string ToString()
        {
            return "Column<" + typeof(T).Name + "> [" + this.Index + "] " + this.Name;
        }
    }
}
