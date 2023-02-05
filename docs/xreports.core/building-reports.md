# Building Reports

## Report Flow

(using report schema builder) → Report Schema → (using data) → Generic Report Table → (converter) → Typed Report Table → (writer) → Output

Converting of generic report table to typed one is optional. It's possible to work with it. Examples given here do not use conversion.

## Building Report Schema

Report schema is object containing information what columns/rows report has, what properties they have, what additional actions are to be taken. It's quite complex object, so it's more convenient to use schema builders - classes providing handy way to build report schema.

There are 2 schema builders: VerticalReportSchemaBuilder and HorizontalReportSchemaBuilder. Vertical report is report having fixed number of columns and dynamic number of rows. Horizontal report, on the other hand, has fixed number of rows and dynamic number of columns.

Both builders are generic classes. It means that you need to specify type of source. There is special IDataReader source type which will be discussed later.

## Vertical Report

Vertical report consists of columns. Each column has to have:
- ID (optional) - column ID, can be used to unambiguously refer to column
- title (required) - title of the column, will be displayed in header cell  
cannot be null, but can be empty string
can be used to refer to column
- report cells provider (required) - the class responsible for providing report cells  
more on this later

### Simple Example

Let's dive into building report by looking at code.

```c#
// Model describing report data source item.
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

// Can use dynamic if you don't know type of data.
VerticalReportSchemaBuilder<Customer> builder = new VerticalReportSchemaBuilder<Customer>();

// Specify columns.
builder.AddColumn("Name", (Customer x) => x.Name);
builder.AddColumn("Email", (Customer x) => x.Email);
builder.AddColumn("Age", (Customer x) => x.Age);

// Build schema.
VerticalReportSchema<Customer> schema = builder.BuildSchema();

// Data source is IEnumerable which allows iterating over large
// collection without having to load it into memory.
IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23 },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22 },
};

// Report table contains header and body of the report.
IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

// Report can have more than one header row. More on this under section "Complex Header".
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 3 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Name                 | Email                | Age                 
------------------------------------------------------------------
John Doe             | john@example.com     | 23                  
Jane Doe             | jane@example.com     | 22                  
*/
```

You can add columns using following methods:

```c#
// Add column to the end.
builder.AddColumn("Name", (Customer x) => x.Name);

// Insert column at specified position (0-based).
builder.InsertColumn(0, "Name", (Customer x) => x.Name);

// Insert column before another column.
// Here column Name will be added before column with title "Age".
builder.InsertColumnBefore("Age", "Name", (Customer x) => x.Name);

// Any of the methods above can be used with column ID specified
// right before title.
// ID does not have to be the same as column title, but it has to be
// unique among all column IDs.
builder.InsertColumn(0, new ColumnId("Name"), "Name", (Customer x) => x.Name);

// Also it is possible to use column ID for inserting column
// before another one.
builder.InsertColumnBefore(new ColumnId("Age"), "Name", (Customer x) => x.Name);
```

### IDataReader

Special case of data source is System.Data.IDataReader. In this case columns callbacks will get IDataReader as argument.

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

await using SqliteConnection connection = new SqliteConnection("Data Source=:memory:");

await connection.OpenAsync();

// Setup database
await connection.ExecuteAsync("CREATE TABLE Customers(Name text, Email text, Age integer)");
await connection.ExecuteAsync(
    @"INSERT INTO Customers(Name, Email, Age) VALUES
        ('John Doe', 'john@example.com', 23),
        ('Jane Doe', 'jane@example.com', 22)"
);

// Type of data source is IDataReader.
VerticalReportSchemaBuilder<IDataReader> builder = new VerticalReportSchemaBuilder<IDataReader>();

// Callbacks accept IDataReader.
builder.AddColumn("Name", (IDataReader x) => x.GetString(0));
builder.AddColumn("Email", (IDataReader x) => x.GetString(1));
builder.AddColumn("Age", (IDataReader x) => x.GetInt32(2));

// Build schema.
VerticalReportSchema<IDataReader> schema = builder.BuildSchema();

// Get IDataReader.
IDataReader dataReader = await connection.ExecuteReaderAsync("SELECT Name, Email, Age FROM Customers", CommandBehavior.CloseConnection);
IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataReader);

// Print report.
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 3 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Name                 | Email                | Age                 
------------------------------------------------------------------
John Doe             | john@example.com     | 23                  
Jane Doe             | jane@example.com     | 22                  
*/
```

### Complex Header

Sometimes report should have columns headers grouped. For example, report should group columns related to customer personal information.

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
	public string JobTitle { get; set; }
	public decimal Salary { get; set; }
}

VerticalReportSchemaBuilder<Customer> builder = new VerticalReportSchemaBuilder<Customer>();
builder.AddColumn("Name", (Customer x) => x.Name);
builder.AddColumn("Email", (Customer x) => x.Email);
builder.AddColumn("Age", (Customer x) => x.Age);
builder.AddColumn(new ColumnId("Job"), "Job", (Customer x) => x.JobTitle);
builder.AddColumn(new ColumnId("Salary"), "Salary ($)", (Customer x) => x.Salary);

// Complex header rows have 0-based index indicating order of the row (0 - top).
// Add complex header by column indexes (0-based): from-to.
builder.AddComplexHeader(0, "Customer Information", 0, 4);
// Or by column titles.
// Code below combines columns from Name to Age under Personal Information.
builder.AddComplexHeader(1, "Personal Information", "Name", "Age");
// Also you can use column ID.
builder.AddComplexHeader(1, "Job Info", new ColumnId("Job"), new ColumnId("Salary"));

// Build schema.
VerticalReportSchema<Customer> schema = builder.BuildSchema();

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23, JobTitle = "Manager", Salary = 1500m },
    new Customer { Name = "David Doe", Email = "david@example.com", Age = 22, JobTitle = "Director", Salary = 2000m },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row
		.Where(c => c != null) // spanned cell is null (!), we don't want to print them, so just skip
		.Select(c => string.Format($"{{0,-{23*c.ColumnSpan - 3}}}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 5 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Customer Information                                                                                            
Personal Information                                               | Job Info                                   
Name                 | Email                | Age                  | Job                  | Salary ($)          
----------------------------------------------------------------------------------------------------------------
John Doe             | john@example.com     | 23                   | Manager              | 1500                
David Doe            | david@example.com    | 22                   | Director             | 2000                
*/
```

## Horizontal Report

### Simple Example

```c#
// Model describing report data source item.
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

HorizontalReportSchemaBuilder<Customer> builder = new HorizontalReportSchemaBuilder<Customer>();

// Specify rows.
builder.AddRow("Name", (Customer x) => x.Name);
builder.AddRow("Email", (Customer x) => x.Email);
builder.AddRow("Age", (Customer x) => x.Age);

// Build schema.
HorizontalReportSchema<Customer> schema = builder.BuildSchema();

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23 },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22 },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

// Horizontal table does not have header row.
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Name                 | John Doe             | Jane Doe            
Email                | john@example.com     | jane@example.com    
Age                  | 23                   | 22                  
*/
```

### Header Row

Often header is required in horizontal report. Here is how to do this.

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

HorizontalReportSchemaBuilder<Customer> builder = new HorizontalReportSchemaBuilder<Customer>();

// Add header row. There might be several header rows if needed.
builder.AddHeaderRow(string.Empty, (Customer x) => x.Name.ToUpperInvariant());

// Add body rows.
builder.AddRow("Email", (Customer x) => x.Email);
builder.AddRow("Age", (Customer x) => x.Age);

// Build schema.
HorizontalReportSchema<Customer> schema = builder.BuildSchema();

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23 },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22 },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 3 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
                     | JOHN DOE             | JANE DOE            
------------------------------------------------------------------
Email                | john@example.com     | jane@example.com    
Age                  | 23                   | 22                  
*/
```

### Complex Header

```c#
class Customer
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

HorizontalReportSchemaBuilder<Customer> builder = new HorizontalReportSchemaBuilder<Customer>();

// Specify rows.
builder.AddRow("Name", (Customer x) => x.Name);
builder.AddRow("Phone", (Customer x) => x.Phone);
builder.AddRow("Email", (Customer x) => x.Email);
builder.AddRow("Age", (Customer x) => x.Age);

// Add complex header. For horizontal report header cells are at left.
builder.AddComplexHeader(0, "Contact Info", "Phone", "Email");

// Build schema.
HorizontalReportSchema<Customer> schema = builder.BuildSchema();

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23, Phone = "1111111111" },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22, Phone = "2222222222" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);
		
// Print. Need to consider spanned cells.
List<string> cellsTexts = new List<string>();
foreach (IEnumerable<ReportCell> row in reportTable.Rows)
{
    int columnSpan = 1;
    cellsTexts.Clear();
    foreach (ReportCell cell in row)
    {
        // spanned cell
        if (cell == null)
        {
            if (columnSpan > 1)
            {
                columnSpan--;
            }
            else
            {
                cellsTexts.Add(string.Format("{0,-20}", string.Empty));
            }
        }
        else
        {
            cellsTexts.Add(string.Format($"{{0,-{cell.ColumnSpan * 23 - 3}}}", cell.GetValue<string>()));

            columnSpan = cell.ColumnSpan;
        }
    }

    Console.WriteLine(string.Join(" | ", cellsTexts));
}

/*
Name                                        | John Doe             | Jane Doe            
Contact Info         | Phone                | 1111111111           | 2222222222          
                     | Email                | john@example.com     | jane@example.com    
Age                                         | 23                   | 22                  
*/
```
