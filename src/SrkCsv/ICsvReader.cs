
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// This interface allows to mock the CsvReader class in unit tests.
    /// </summary>
    public interface ICsvReader<T>
    {
        char CellSeparator { get; set; }
        bool HasHeaderLine { get; set; }
        CultureInfo Culture { get; set; }
        Table<T> ReadToEnd(TextReader reader);
    }
}
