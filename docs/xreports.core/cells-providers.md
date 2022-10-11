# Cells Providers

Cells providers are classes that generate cells for column or row depending on whether report is vertical or horizontal.

## Built-in Providers

All built-in cells providers are inherited from `ReportCellsProvider<TSourceEntity, TValue>`. It provides ability to add properties and processors to cells.

### ComputedValueReportCellsProvider

Probably, the most used cells provider. When you add column or row supplying callback, internally instance of this ComputedValueReportCellsProvider class is created.

```c#
builder.AddColumn("Name", (Customer c) => c.Name);
// is the same as
builder.AddColumn(new ComputedValueReportCellsProvider<Customer, string>("Name", (Customer c) => c.Name));
```

This provider accepts callback that returns cell value.

### EmptyCellsProvider

Sometimes you might want to have empty row or column. In this case you may use this cells provider.

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Score { get; set; }
    public string Comment { get; set; }
}

HorizontalReportSchemaBuilder<Customer> builder = new HorizontalReportSchemaBuilder<Customer>();

builder.AddHeaderRow(string.Empty, (Customer c) => c.Name);

// Add empty row to separate rows visually.
builder.AddRow(new EmptyCellsProvider<Customer>("Personal Info"));
builder.AddRow("Email", (Customer c) => c.Email);

// Another "group" of rows.
builder.AddRow(new EmptyCellsProvider<Customer>("Review Info"));
builder.AddRow("Score", (Customer c) => c.Score);
builder.AddRow("Comment", (Customer c) => c.Comment);

HorizontalReportSchema<Customer> schema = builder.BuildSchema();

Customer[] customers = new Customer[]
{
    new Customer() { Name = "John", Email = "john@example.com", Score = 10, Comment = "Great!" },
    new Customer() { Name = "Jane", Email = "jane@example.com", Score = 2, Comment = "Couldn't be worse" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

// Print.
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 3 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
                     | John                 | Jane                
------------------------------------------------------------------
Personal Info        |                      |                     
Email                | john@example.com     | jane@example.com    
Review Info          |                      |                     
Score                | 10                   | 2                   
Comment              | Great!               | Couldn't be worse
*/
```

### ValueProviderReportCellsProvider

It returns cells from data returned by supplied IValueProvider. There is short form for adding this cells provider.

```c#
builder.AddColumn("#", new SequentialNumberValueProvider());
// is the same as
builder.AddColumn(new ValueProviderReportCellsProvider<Customer, int>("#", new SequentialNumberValueProvider()));
```

## Custom Cells Providers

Let's imagine that we have column in report that shows record status: active/inactive/deleted. We might have many different reports with this column. In order to not duplicate logic for displaying and formatting this column we will create our custom cells provider and use it in different reports.

```c#
// Should implement IReportCellsProvider interface.
class StatusCellsProvider<TData> : IReportCellsProvider<TData>
{
    // Cell content variations.
    private const string ActiveText = "☑";
    private const string InactiveText = "☐";
    private const string DeletedText = "☒";

    private readonly ReportCell activeReportCell = ReportCell.FromValue(ActiveText);
    private readonly ReportCell inactiveReportCell = ReportCell.FromValue(InactiveText);
    private readonly ReportCell deletedReportCell = ReportCell.FromValue(DeletedText);

    // As our cells provider is generic, it does not know how to determine cell
    // status. So it's delegated to these functions.
    private readonly Func<TData, bool> isActive;
    private readonly Func<TData, bool> isDeleted;

    public StatusCellsProvider(string title, Func<TData, bool> isActive, Func<TData, bool> isDeleted)
    {
        this.Title = title;
        this.isActive = isActive;
        this.isDeleted = isDeleted;
    }

    #region IReportCellsProvider

    // Every cells provider should have title. It will be displayed at the top
    // of report of at the left column depending on if report is vertical or
    // horizontal.
    public string Title { get; }

    // Returns cell for given entity.
    public ReportCell GetCell(TData entity)
    {
        return this.isDeleted(entity) ?
            this.deletedReportCell :
            this.isActive(entity) ?
                this.activeReportCell :
                this.inactiveReportCell;
    }

    #endregion
}
```

Now we have our cells provider, so let's apply it to some reports:

```c#
class Product
{
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}

VerticalReportSchemaBuilder<Product> productBuilder = new VerticalReportSchemaBuilder<Product>();

productBuilder.AddColumn(new StatusCellsProvider<Product>("Status", p => p.IsActive, p => p.IsDeleted));
productBuilder.AddColumn("Name", (Product p) => p.Name);

VerticalReportSchema<Product> productSchema = productBuilder.BuildSchema();

Product[] products = new Product[]
{
    new Product() { Name = "Notebook" },
    new Product() { Name = "PC", IsDeleted = true },
    new Product() { Name = "iPhone", IsActive = false },
};

IReportTable<ReportCell> reportTable = productSchema.BuildReportTable(products);

// Print.
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 2 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Status               | Name                
-------------------------------------------
☑                    | Notebook            
☒                    | PC                  
☐                    | iPhone              
*/
```

Another report might look like following:

```c#
class User
{
    public string Username { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsDeleted { get; set; }
}

VerticalReportSchemaBuilder<User> userBuilder = new VerticalReportSchemaBuilder<User>();

userBuilder.AddColumn(new StatusCellsProvider<User>("User status", p => !p.IsBlocked, p => p.IsDeleted));
userBuilder.AddColumn("Username", (User p) => p.Username);

VerticalReportSchema<User> userSchema = userBuilder.BuildSchema();

User[] users = new User[]
{
    new User() { Username = "admin" },
    new User() { Username = "user1", IsDeleted = true },
    new User() { Username = "user2", IsBlocked = true },
};

IReportTable<ReportCell> reportTable = userSchema.BuildReportTable(users);

// Print.
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 2 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
User status          | Username            
-------------------------------------------
☑                    | admin               
☒                    | user1               
☐                    | user2               
*/
```

Having logic implemented in separate class allows changing status representation later easily.
