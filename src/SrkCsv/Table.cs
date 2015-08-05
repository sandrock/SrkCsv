
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Table
    {
        internal IList<Column> columns = new List<Column>();

        public Table()
        {
        }

        public IList<Column> Columns
        {
            get { return this.columns; }
            set { this.columns = value; }
        }

        public List<Row> Rows { get; set; }

        internal virtual Table Clone(bool copyRows)
        {
            return new Table
            {
                Columns = this.Columns.ToList(),
            };
        }

        internal virtual Cell CreateCell(Column col, Row row, string value)
        {
            return new Cell
            {
                Column = col,
                Row = row,
                Value = value,
            };
        }

        internal virtual Row CreateRow(int index, CultureInfo culture, bool isHeader)
        {
            return new Row(index)
            {
                Culture = culture,
                IsHeader = isHeader,
            };
        }
    }

    public class Table<T> : Table
    {
        public Table()
        {
        }

        public Table AddColumn(int index, string name, Action<Cell<T>> action)
        {
            var col = new Column(index, name);
            col.Transform = new Action<Cell>(x => action((Cell<T>)x));
            this.columns.Add(col);
            return this;
        }

        internal override Table Clone(bool copyRows)
        {
            return new Table<T>
            {
                Columns = this.Columns.ToList(),
            };
        }

        internal override Cell CreateCell(Column col, Row row, string value)
        {
            return new Cell<T>
            {
                Column = col,
                Row = row,
                Value = value,
                Target = Activator.CreateInstance<T>(),
            };
        }

        internal override Row CreateRow(int index, CultureInfo culture, bool isHeader)
        {
            return new Row<T>(index)
            {
                Culture = culture,
                IsHeader = isHeader,
            };
        }
    }

    public delegate void ParseCellDelegate(Cell cell);
}
