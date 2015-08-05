
namespace SrkCsv.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;

    [TestClass]
    public class CsvReaderTests
    {
        [TestClass]
        public class ReadToEndMethod
        {
            [TestMethod]
            public void Csv1_Raw()
            {
                var table = new Table();
                table.Columns.Add(new Column(0, "Firstname"));
                table.Columns.Add(new Column(1, "Lastname"));
                table.Columns.Add(new Column(2, "City"));
                table.Columns.Add(new Column(3, "Age"));
                var target = new CsvReader(table);
                target.CellSeparator = ',';
                target.HasHeaderLine = true;
                var reader = new StringReader(CsvFiles.Csv1);
                var result = target.ReadToEnd(reader);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Columns);
                Assert.IsNotNull(result.Rows);
                Assert.AreEqual(4, result.Rows.Count);
                Assert.AreEqual("Firstname", result.Rows[0].Cells[0].Value);
                Assert.IsTrue(result.Rows[0].IsHeader);
                Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
                Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
                Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
                Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
            }

            [TestMethod]
            public void Csv1_Transformed()
            {
                var table = new Table<Csv1Row>();
                table.AddColumn(0, "Firstname", x => x.Target.FirstName = x.Value);
                table.AddColumn(1, "Lastname", x => x.Target.LastName = x.Value);
                table.AddColumn(2, "City", x => x.Target.City = x.Value);
                table.AddColumn(3, "Age", x => x.Target.Age = int.Parse(x.Value));
                var target = new CsvReader(table);
                target.CellSeparator = ',';
                target.HasHeaderLine = true;
                var reader = new StringReader(CsvFiles.Csv1);
                var result = target.ReadToEnd(reader);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Columns);
                Assert.IsNotNull(result.Rows);
                Assert.AreEqual(4, result.Rows.Count);
                Assert.AreEqual("Firstname", result.Rows[0].Target);
                Assert.IsTrue(result.Rows[0].IsHeader);
                Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
                Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
                Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
                Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
            }
        }
    }
}
