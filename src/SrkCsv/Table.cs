
namespace SrkCsv
{
    using SrkCsv.Internals;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Table information and data (non-generic).
    /// </summary>
    public class Table : Table<Nothing>
    {
    }

    /// <summary>
    /// Table information and data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Table<T>
    {
        internal IList<Column<T>> columns = new List<Column<T>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Table{T}"/> class.
        /// </summary>
        public Table()
        {
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public IList<Column<T>> Columns
        {
            get { return this.columns; }
            set { this.columns = value; }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        public List<Row<T>> Rows { get; set; }

        /// <summary>
        /// Adds a column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Table<T> AddColumn(int index, string name)
        {
            this.AddColumn(index, name, default(Predicate<Cell<T>>));
            return this;
        }

        /// <summary>
        /// Adds a column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public Table<T> AddColumn(int index, string name, Predicate<Cell<T>> action)
        {
            var col = new Column<T>(index, name);
            col.Transform = action;
            this.columns.Add(col);
            return this;
        }

        /// <summary>
        /// Adds a column.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public Table<T> AddColumn(int index, string name, Action<Cell<T>> action)
        {
            var col = new Column<T>(index, name);
            col.Transform = new Predicate<Cell<T>>(x =>
            {
                action(x);
                return true;
            });
            this.columns.Add(col);
            return this;
        }

        public override string ToString()
        {
            return "Table<" + typeof(T).Name + "> Columns:" + (this.columns != null ? this.columns.Count.ToString() : "none") + " Rows:" + (this.Rows != null ? this.Rows.Count.ToString() : "none");
        }

        internal Table<T> Clone(bool copyRows)
        {
            return new Table<T>
            {
                Columns = this.Columns.ToList(),
            };
        }

        internal Cell<T> CreateCell(Column<T> col, Row<T> row, string value)
        {
            return new Cell<T>
            {
                Column = col,
                Row = row,
                Value = value,
                Target = row.Target,
            };
        }

        internal Row<T> CreateRow(int index, CultureInfo culture, bool isHeader)
        {
            return new Row<T>(index)
            {
                Culture = culture,
                IsHeader = isHeader,
                Target = Activator.CreateInstance<T>(),
            };
        }
    }
}
