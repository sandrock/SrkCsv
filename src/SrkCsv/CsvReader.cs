
namespace SrkCsv
{
    using SrkCsv.Internals;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Basic CSV reader.
    /// </summary>
    public class CsvReader : CsvReader<Nothing>
    {
        public CsvReader()
            : base()
        {
        }

        public CsvReader(Table table)
            : base(table)
        {
        }
    }

    /// <summary>
    /// CSV reader with support for a target object type.
    /// </summary>
    /// <typeparam name="T">the target object type</typeparam>
    public class CsvReader<T>
    {
        private readonly Table<T> table;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader{T}"/> class.
        /// </summary>
        public CsvReader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvReader{T}"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public CsvReader(Table<T> table)
        {
            this.table = table;
        }

        /// <summary>
        /// Gets or sets the cell separator.
        /// </summary>
        public char CellSeparator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [has header line].
        /// </summary>
        public bool HasHeaderLine { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get { return this.culture; }
            set { this.culture = value; }
        }

        /// <summary>
        /// Reads to end.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public Table<T> ReadToEnd(TextReader reader)
        {
            var data = new List<List<string>>();

            var errors = new List<string>();
            this.ParseToEnd(reader, data, errors);
            var table = this.table.Clone(false);
            table.Rows = new List<Row<T>>(data.Count);
            this.Transform(data, errors, table);
            return table;
        }

        private void ParseToEnd(TextReader reader, List<List<string>> data, List<string> errors)
        {
            errors = new List<string>();
            var err = new Action<int, string>((int lineIndex1, string error) =>
            {
                errors.Add("CSV:" + lineIndex1 + " Error: " + error);
            });

            var warnings = new List<string>();
            var warn = new Action<int, string>((int lineIndex1, string error) =>
            {
                warnings.Add("CSV:" + lineIndex1 + " Warning: " + error);
            });


            var separator = this.CellSeparator;
            string line;
            int lineIndex = -1;
            bool inCell = false, inQuotes = false;
            StringBuilder cellBuilder = null;
            int startOfCell = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineIndex++;

                if (string.IsNullOrEmpty(line))
                {
                    warn(lineIndex, "Line is empty");
                    continue;
                }

                var dataList = new List<string>();
                data.Add(dataList);

                inCell = false;
                inQuotes = false;
                startOfCell = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    var c = line[i];

                    if (!inCell)
                    {
                        if (c == separator)
                        {
                            inCell = false;
                            startOfCell = i;
                        }
                        else
                        {
                            startOfCell = i;
                            if (c == '"')
                            {
                                startOfCell = i + 1;
                                inQuotes = true;
                                cellBuilder = new StringBuilder(line.Length - i);
                            }

                            inCell = true;
                        }
                    }
                    else if (inQuotes)
                    {
                        if (c == '"' && line.Length > (i + 1) && line[i + 1] == '"')
                        {
                            // escaped quote => ignore
                            i++;
                            cellBuilder.Append(c);
                        }
                        else if (c == '"' && line.Length > (i + 1) && line[i + 1] == separator)
                        {
                            // end of quoted cell
                            i++;
                            inQuotes = false;
                            inCell = false;
                        }
                        else
                        {
                            cellBuilder.Append(c);
                        }
                    }

                    if (c == separator)
                    {
                        if (inQuotes)
                        {
                            // do nothing here
                        }
                        else if (inCell)
                        {
                            inCell = false;
                        }
                        else
                        {
                            startOfCell = i;
                        }
                    }

                    if (!inCell)
                    {
                        // end of cell => capture
                        if (cellBuilder != null)
                        {
                            // when the line is quoted, we use a StringBuilder to skip some chars
                            Debug.Assert(c == '"', "CsvUserImport at end of cell: cellBuilder is set but in unquoted cell");
                            dataList.Add(cellBuilder.ToString());
                            cellBuilder = null;
                        }
                        else
                        {
                            dataList.Add(line.Substring(startOfCell, i - startOfCell));
                        }
                    }

                    if ((i + 1) == line.Length)
                    {
                        // end of line => capture
                        if (inCell)
                        {
                            dataList.Add(line.Substring(startOfCell, i - startOfCell + 1));
                        }
                        else
                        {
                            // when last entry is empty
                            dataList.Add(string.Empty);
                        }

                        cellBuilder = null;
                    }
                }
            }

            int colCount = -1;

            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                {
                    colCount = data[i].Count;
                }
                else if (data[i].Count != colCount)
                {
                    err(i, "Invalid cell count '" + data[i].Count + "'; expected '" + colCount + "'");
                }
            }

            if (errors.Count > 0)
            {
                throw new CsvParseException("Parse failed with " + errors.Count + " errors.", errors);
            }
        }

        private void Transform(List<List<string>> data, List<string> errors, Table<T> table)
        {
            var err = new Action<int, string>((int line, string error) =>
            {
                errors.Add("CSV:" + line + " " + error);
            });

            for (int i = 0; i < data.Count; i++)
            {
                var line = data[i];

                try
                {
                    var row = this.table.CreateRow(i, culture, this.HasHeaderLine && i == 0);
                    table.Rows.Add(row);

                    var cells = new List<Cell<T>>(table.Columns.Count);
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        var col = table.Columns[c];
                        var cell = line.Count >= (col.Index + 1) ? line[col.Index] : null;
                        if (cell == null)
                        {
                            err(i, "CSV row " + i + " does not contain column '" + col.Name + "' because index " + col.Index + " does not exist");
                        }

                        var cellObj = this.table.CreateCell(col, row, cell);
                        row.Target = cellObj.Target;
                        if (!row.IsHeader && col.Transform != null && !col.Transform(cellObj))
                        {
                            errors.Add("Parse failed at row:" + i + " col:" + c);
                        }

                        cells.Add(cellObj);
                    }

                    ////for (int c = 0; c < cells.Count; c++)
                    ////{
                    ////    var cell = cells[c];
                    ////    cell.Column.Parse(cell);
                    ////    cell.Dispose();
                    ////}

                    row.Cells = cells;
                }
                catch (Exception ex)
                {
                    err(i, "Internal error: " + ex.Message);
                }
            }

            if (errors.Count == 1)
            {
                throw new CsvParseException(errors[0], errors);
            }
            else if (errors.Count > 1)
            {
                throw new CsvParseException("Parse failed with " + errors.Count + " errors.", errors);
            }
        }

        private void EnsureTable()
        {
            if (this.table == null)
                throw new InvalidOperationException("Table definition not set");
        }
    }
}
