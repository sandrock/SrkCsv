
namespace SrkCsv
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Cell<T>
    {
        private bool isDisposed;

        public Cell()
        {
        }

        public Row<T> Row { get; set; }
        public Column<T> Column { get; set; }
        public string Value { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        public T Target { get; set; }
    }
}
