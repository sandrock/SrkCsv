
namespace SrkCsv
{
    using SrkCsv.Internals;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Table : Table<Nothing>
    {
    }

    public class Table<T>
    {
        internal IList<Column<T>> columns = new List<Column<T>>();

        public Table()
        {
        }

        public IList<Column<T>> Columns
        {
            get { return this.columns; }
            set { this.columns = value; }
        }

        public List<Row<T>> Rows { get; set; }

        public Table<T> AddColumn(int index, string name)
        {
            this.AddColumn(index, name, default(Predicate<Cell<T>>));
            return this;
        }

        public Table<T> AddColumn(int index, string name, Predicate<Cell<T>> action)
        {
            var col = new Column<T>(index, name);
            col.Transform = action;
            this.columns.Add(col);
            return this;
        }

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

    public delegate void ParseCellDelegate<T>(Cell<T> cell);
}
