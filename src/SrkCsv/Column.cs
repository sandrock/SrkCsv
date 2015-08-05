
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Column<T>
    {
        private Action<Cell<T>> parser;

        public Column(int index, string col)
        {
            this.Index = index;
            this.Name = col;

            ////this.CreateParser();
        }

        public int Index { get; set; }

        public string Name { get; set; }

        internal Predicate<Cell<T>> Transform { get; set; }

        ////public static IList<ColumnParser> Parsers
        ////{
        ////    get { return parsers.Select(p => new ColumnParser { Name = p.Key, }).ToArray(); }
        ////}

        ////public void Parse(Cell<T> cell)
        ////{
        ////    if (this.parser != null)
        ////    {
        ////        this.parser(cell);
        ////    }
        ////}

        ////private static void AddParser(ColumnParser parser)
        ////{
        ////    parsers.Add(parser.Name.ToLowerInvariant(), parser);
        ////}

        ////private void CreateParser()
        ////{
        ////    var lowerName = this.Name.ToLowerInvariant();
        ////    if (parsers.ContainsKey(lowerName))
        ////        this.parser = parsers[lowerName].Action;
        ////    else
        ////        throw new InvalidOperationException("Unrecognized column '" + this.Name + "' ");
        ////}
    }
}
