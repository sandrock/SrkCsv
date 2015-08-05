
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ColumnParser
    {
        public ColumnParser()
        {
        }

        public ColumnParser(string name, Action<Cell> action)
        {
            this.Name = name;
            this.Action = action;
        }

        public string Name { get; set; }

        public Action<Cell> Action { get; set; }
    }
}
