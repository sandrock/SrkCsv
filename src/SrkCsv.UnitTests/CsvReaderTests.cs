
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
                table.AddColumn(0, "Firstname");
                table.AddColumn(1, "Lastname");
                table.AddColumn(2, "City");
                table.AddColumn(3, "Age");
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
                table.AddColumn(3, "Age", x =>
                {
                    int age;
                    var ok = int.TryParse(x.Value, out age);
                    x.Target.Age = age;
                    return ok;
                });
                var target = new CsvReader<Csv1Row>(table);
                target.CellSeparator = ',';
                target.HasHeaderLine = true;
                var reader = new StringReader(CsvFiles.Csv1);
                var result = target.ReadToEnd(reader);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Columns);
                Assert.IsNotNull(result.Rows);
                Assert.AreEqual(4, result.Rows.Count);
                Assert.AreEqual("Firstname", result.Rows[0].Cells[0].Value);
                ////Assert.AreEqual("Firstname", result.Rows[0].Target.FirstName);
                Assert.IsTrue(result.Rows[0].IsHeader);
                Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
                Assert.AreEqual("Gregory", result.Rows[1].Target.FirstName);
                Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
                Assert.AreEqual("Gorgini", result.Rows[2].Target.LastName);
                Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
                Assert.AreEqual("Valenciennes", result.Rows[3].Target.City);
                Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
                Assert.AreEqual(100, result.Rows[3].Target.Age);
            }

            [TestMethod]
            public void Csv1_Transformed_Semicolon()
            {
                var table = new Table<Csv1Row>();
                table.AddColumn(0, "Firstname", x => x.Target.FirstName = x.Value);
                table.AddColumn(1, "Lastname", x => x.Target.LastName = x.Value);
                table.AddColumn(2, "City", x => x.Target.City = x.Value);
                table.AddColumn(3, "Age", x =>
                {
                    int age;
                    var ok = int.TryParse(x.Value, out age);
                    x.Target.Age = age;
                    return ok;
                });
                var target = new CsvReader<Csv1Row>(table);
                target.CellSeparator = ';';
                target.HasHeaderLine = true;
                var reader = new StringReader(CsvFiles.Csv1.Replace(CsvFiles.Csv1Separator, ";"));
                var result = target.ReadToEnd(reader);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Columns);
                Assert.IsNotNull(result.Rows);
                Assert.AreEqual(4, result.Rows.Count);
                Assert.AreEqual("Firstname", result.Rows[0].Cells[0].Value);
                ////Assert.AreEqual("Firstname", result.Rows[0].Target.FirstName);
                Assert.IsTrue(result.Rows[0].IsHeader);
                Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
                Assert.AreEqual("Gregory", result.Rows[1].Target.FirstName);
                Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
                Assert.AreEqual("Gorgini", result.Rows[2].Target.LastName);
                Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
                Assert.AreEqual("Valenciennes", result.Rows[3].Target.City);
                Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
                Assert.AreEqual(100, result.Rows[3].Target.Age);
            }
        }

        [TestClass]
        public class ReadLineMethod
        {
            [TestMethod]
            public void Csv1_Raw()
            {
                var table = new Table();
                table.AddColumn(0, "Firstname");
                table.AddColumn(1, "Lastname");
                table.AddColumn(2, "City");
                table.AddColumn(3, "Age");
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
                table.AddColumn(3, "Age", x =>
                {
                    int age;
                    var ok = int.TryParse(x.Value, out age);
                    x.Target.Age = age;
                    return ok;
                });
                var target = new CsvReader<Csv1Row>(table);
                target.CellSeparator = ',';
                target.HasHeaderLine = true;
                var reader = new StringReader(CsvFiles.Csv1);
                var result = target.ReadToEnd(reader);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Columns);
                Assert.IsNotNull(result.Rows);
                Assert.AreEqual(4, result.Rows.Count);
                Assert.AreEqual("Firstname", result.Rows[0].Cells[0].Value);
                ////Assert.AreEqual("Firstname", result.Rows[0].Target.FirstName);
                Assert.IsTrue(result.Rows[0].IsHeader);
                Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
                Assert.AreEqual("Gregory", result.Rows[1].Target.FirstName);
                Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
                Assert.AreEqual("Gorgini", result.Rows[2].Target.LastName);
                Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
                Assert.AreEqual("Valenciennes", result.Rows[3].Target.City);
                Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
                Assert.AreEqual(100, result.Rows[3].Target.Age);
            }

            [TestMethod]
            public void Csv1_Transformed_Semicolon()
            {
                var table = new Table<Csv1Row>();
                table.AddColumn(0, "Firstname", x => x.Target.FirstName = x.Value);
                table.AddColumn(1, "Lastname", x => x.Target.LastName = x.Value);
                table.AddColumn(2, "City", x => x.Target.City = x.Value);
                table.AddColumn(3, "Age", x =>
                {
                    int age;
                    var ok = int.TryParse(x.Value, out age);
                    x.Target.Age = age;
                    return ok;
                });
                var target = new CsvReader<Csv1Row>(table);
                target.CellSeparator = ';';
                target.HasHeaderLine = true;
                var reader = new StringReader(CsvFiles.Csv1.Replace(CsvFiles.Csv1Separator, ";"));
                var result = target.ReadToEnd(reader);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Columns);
                Assert.IsNotNull(result.Rows);
                Assert.AreEqual(4, result.Rows.Count);
                Assert.AreEqual("Firstname", result.Rows[0].Cells[0].Value);
                ////Assert.AreEqual("Firstname", result.Rows[0].Target.FirstName);
                Assert.IsTrue(result.Rows[0].IsHeader);
                Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
                Assert.AreEqual("Gregory", result.Rows[1].Target.FirstName);
                Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
                Assert.AreEqual("Gorgini", result.Rows[2].Target.LastName);
                Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
                Assert.AreEqual("Valenciennes", result.Rows[3].Target.City);
                Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
                Assert.AreEqual(100, result.Rows[3].Target.Age);
            }
        }

        [TestClass]
        public class SampleCsvFiles
        {
            [TestMethod]
            public void FranceLibraries()
            {
                // this test resolved a bug (line 98814)
                // a separator within a quoted value did make the algorithm quit the cell
                // https://www.data.gouv.fr/s/resources/adresses-des-bibliotheques-publiques/20151027-100052/adresses_bibliotheques_2014.csv
                string sample = @"insee,libelle1,libelle2,voie_num,voie_type,voie_nom,local,voie,CPBIBLIO,CEDEXB,BP,ville,DEPT,REGION,population_legale
01004,Médiathèque Municipale,La Grenette,10,rue,Amédée Bonnet,,10 rue Amédée Bonnet,01500,,,Ambérieu-en-Bugey,01,82,14347
01081,""Bibliothèque """"le Champ Du Livre"""""",,564,route,des Burgondes,,564 rue des Burgondes,01410,,,Champfromier,01,82,694
35195,""""""la Majuscule"""" Bibliothèque Municipale"",,10,rue,du Clos Gérard,,10 rue du Clos Gérard,35440,,,Montreuil-sur-Ille,35,53,2153
38528,Bibliotheque Municipale,""""""LA LICONTE"""""",195,Promenade des Noyers,,,195 Promenade des Noyers,38410,,,Vaulnaveys-le-Bas,38,82,1227
98814,""Bibliotheque Provinciale, Service Culturel"",Association Löhna - Médiathèque Löhna,,WE,,B.P. 752,WE,98820,,BP 50,Lifou,988,NC,21244
";

                var table = new Table();
                table.AddColumn(1, "name1");
                table.AddColumn(2, "name2");
                var target = new CsvReader(table);
                target.CellSeparator = ',';
                target.HasHeaderLine = true;
                var reader = new StringReader(sample);
                var result = target.ReadToEnd(reader);

                Assert.AreEqual(6, result.Rows.Count);
                Assert.AreEqual("Bibliothèque \"le Champ Du Livre\"", result.Rows[2].Cells[0].Value);
                Assert.AreEqual("\"la Majuscule\" Bibliothèque Municipale", result.Rows[3].Cells[0].Value);
                Assert.AreEqual("\"LA LICONTE\"", result.Rows[4].Cells[1].Value);
                Assert.AreEqual("Bibliotheque Provinciale, Service Culturel", result.Rows[5].Cells[0].Value);
            }
        }
    }
}
