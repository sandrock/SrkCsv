
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Row<T>
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();
        private int i;
        private IList<Cell<T>> cells = new List<Cell<T>>();

        public Row(int i)
        {
            this.i = i;
            this.Errors = new List<string>();
            this.Warnings = new List<string>();
        }

        public int Index
        {
            get { return this.i; }
        }

        public IList<string> Errors { get; set; }

        public IList<string> Warnings { get; set; }

        public void SetAttachment<T>(T data)
        {
            this.data[typeof(T).FullName] = data;
        }

        public T GetAttachment<T>()
        {
            var key = typeof(T).FullName;
            return this.data.ContainsKey(key) ? (T)this.data[key] : default(T);
        }

        public CultureInfo Culture { get; set; }

        public bool IsHeader { get; set; }

        public IList<Cell<T>> Cells
        {
            get { return this.cells; }
            set { this.cells = value; }
        }

        internal virtual void SetTarget(object target)
        {
        }

        public T Target { get; set; }
    }
}
