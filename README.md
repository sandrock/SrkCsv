# SrkCsv

![SrkCsv logo](https://raw.githubusercontent.com/sandrock/SrkCsv/master/res/logo-64.png)

Super Fast :zap: & Super light :candy: CSV parser that uses lambas :pizza: to create data objects. [nuget: SrkCsv](https://www.nuget.org/packages/SrkCsv/)

```powershell
PM> Install-Package SrkCsv
```

Steps to read your CSV now :zap:
--------------------------------

1. Install the nuget package `SrkCsv`. :balloon:
2. (optional) Have a class with properties that represent a row of your file. :v: Say `YourRowType`.
3. Create a table object.  :seedling:  
   `var table = new Table<YourRowType>();`   
   Omit the generic type if you don't need it.
4. Define the columns you want to parse using :eyes:  
   `table.AddColumn(0, "Firstname", row => row.Target.Firstname = row.Value);`.  
   Omit the third argument if you don't need it to fill a custom object.
5. Create a reader. You may need to configure it. :wrench:  
   `var reader = new CsvReader<YourRowType>(table);`.    
   Omit the generic type if you don't need it.
6. Pass anything inheriting from `TextReader` to `reader.ReadToEnd(reader)`.  
   A `StreamReader` or a `new StringReader(string)` will do the job. :floppy_disk:
7. Your `table` object is now filled.  :clap:  
   Check `table.Rows` for you data.

Example below. 


Why another CSV reader?
------------------------

You can find tons of CSV readers everywhere on the interwebs. But:

- **you won't find a CSV reader with a funny readme.**  :point_up:
- built with lightweight code 🥬
- this package does only :one: thing: parse a CSV. the other packages do a lot more things (write, sort, cache, serialize, coma-tize and so on). So if you like simplicity, here it is.
- it's free!  :beer: (The Unlicense)
- the assembly has a strong name.  :cake:


Example
--------

First, here is a sample/simple CSV:

```
Firstname,Lastname,City,Age
Gregory,Malkov,Paris,20
Matt,Gorgini,Lille,45
Paul,Menier,Valenciennes,100
```

This [unit test](https://github.com/sandrock/SrkCsv/blob/master/src/SrkCsv.UnitTests/CsvReaderTests.cs) will show you how it looks like when using a custom target type ([Csv1Row class](https://github.com/sandrock/SrkCsv/blob/master/src/SrkCsv.UnitTests/Csv1Row.cs)):

```csharp
public class Csv1Row
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public int Age { get; set; }
}

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
    
    // create reader, configure and ReadToEnd
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

This unit test shows that a target class is optional:

```csharp
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
```

And as always...
-----------------

Have a good parse!  :running:

