# SrkCsv

Super Fast & Super light CSV parser that uses lambas to create data objects. [nuget: SrkCsv](https://www.nuget.org/packages/SrkCsv/)

```powershell
PM> Install-Package SrkCsv
```

Steps to read your CSV now:

1. Install the nuget package SrkCsv.
2. Have a class with properties that represent a row of your file.
3. Create a `var table = new Table<YourRowType>()`.
4. Define the columns you want to parse using   
`table.AddColumn(0, "Firstname", row => row.Target.Firstname = row.Value);`.
5. Create a reader `var reader = new CsvReader<YourRowType>(table);`. You may need to configure it.
6. Pass anything inheriting from `TextReader` to `reader.ReadToEnd(reader)`.  
A `StreamReader` or a `StringReader` will do the job.
7. Your `table` object is now filled (check `table.Rows`).

This unit test will show you how it looks like:

```csharp
[TestMethod]
public void Csv1_Transformed()
{
    // describe your csv
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
    
    // create reader and ReadToEnd
    var target = new CsvReader<Csv1Row>(table);
    target.CellSeparator = ',';
    target.HasHeaderLine = true;
    var reader = new StringReader(CsvFiles.Csv1);
    var result = target.ReadToEnd(reader);
    
    // you get a table with columns and rows and cells
    Assert.IsNotNull(result);
    Assert.IsNotNull(result.Columns);
    Assert.IsNotNull(result.Rows);
    Assert.AreEqual(4, result.Rows.Count);
    Assert.AreEqual("Firstname", result.Rows[0].Cells[0].Value);
    Assert.IsTrue(result.Rows[0].IsHeader);
    Assert.IsTrue(result.Rows[0].IsHeader);
    
    // raw string access or strongly-typed target object (thank you lambdas!)
    Assert.AreEqual("Gregory", result.Rows[1].Cells[0].Value);
    Assert.AreEqual("Gregory", result.Rows[1].Target.FirstName);
    Assert.AreEqual("Gorgini", result.Rows[2].Cells[1].Value);
    Assert.AreEqual("Gorgini", result.Rows[2].Target.LastName);
    Assert.AreEqual("Valenciennes", result.Rows[3].Cells[2].Value);
    Assert.AreEqual("Valenciennes", result.Rows[3].Target.City);
    Assert.AreEqual("100", result.Rows[3].Cells[3].Value);
    Assert.AreEqual(100, result.Rows[3].Target.Age);
}
```

```
Firstname,Lastname,City,Age
Gregory,Malkov,Paris,20
Matt,Gorgini,Lille,45
Paul,Menier,Valenciennes,100
```

