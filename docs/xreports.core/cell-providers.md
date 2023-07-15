# Cell Providers

Cells providers are classes that generate cells for column or row depending on whether report is vertical or horizontal.

## Built-in Providers

All built-in cell providers are inherited from `ReportCellProvider<TSourceEntity, TValue>`. It does not create new cell each time, instead it reuses same cell but clears it and sets value provided by inherited classes.

### ComputedValueReportCellProvider

Probably, the most used cell provider. When you add column or row by supplying lambda returning cell value, internally instance of ComputedValueReportCellProvider class is created.

```c#
builder.AddColumn("Name", (Customer c) => c.Name);
// is the same as
builder.AddColumn("Name", new ComputedValueReportCellProvider<Customer, string>((Customer c) => c.Name));
```

### EmptyCellProvider

Sometimes you might want to have empty row or column. In this case you may use this cell provider.

```c#
// Add empty row in horizontal report to separate rows visually.
builder.AddColumn("Personal Info", new EmptyCellProvider<Customer>());
builder.AddColumn("Email", (Customer c) => c.Email);

// Another "group" of rows.
builder.AddColumn("Review Info", new EmptyCellProvider<Customer>());
builder.AddColumn("Score", (Customer c) => c.Score);
builder.AddColumn("Comment", (Customer c) => c.Comment);

/*
|        Personal Info |                      |                      |                      |
|                Email |     john@example.com |    john2@example.com |    john3@example.com |
|          Review Info |                      |                      |                      |
|                Score |                   10 |                    2 |                    7 |
|              Comment |               Great! |    Couldn't be worse |    You can do better |
*/
```

### ValueProviderReportCellProvider

It returns cells from data returned by supplied IValueProvider. There is short form for adding this cell provider.

```c#
builder.AddColumn("#", new SequentialNumberValueProvider());
// is the same as
builder.AddColumn("#", new ValueProviderReportCellProvider<Customer, int>(new SequentialNumberValueProvider()));
```

## Custom Cell Providers

Let's imagine that we have column in report that shows record status: active/inactive/deleted. We might have many different reports with this column. In order to not duplicate logic for displaying and formatting this column we will create our custom cell provider and use it in different reports.

[Working example](samples/cell-providers/XReports.DocsSamples.CellProviders.CustomCellProviders/Program.cs)

```c#
// Should implement IReportCellProvider interface.
internal class StatusCellProvider<TData> : IReportCellProvider<TData>
{
    // Cell content variations.
    private const string ActiveText = "☑";
    private const string InactiveText = "☐";
    private const string DeletedText = "☒";

    private readonly ReportCell activeReportCell;
    private readonly ReportCell inactiveReportCell;
    private readonly ReportCell deletedReportCell;

    // As our cell provider is generic, it does not know how to determine cell
    // status. So it's delegated to these functions.
    private readonly Func<TData, bool> isActive;
    private readonly Func<TData, bool> isDeleted;

    public StatusCellProvider(Func<TData, bool> isActive, Func<TData, bool> isDeleted)
    {
        this.isActive = isActive;
        this.isDeleted = isDeleted;

        this.activeReportCell = new ReportCell();
        this.activeReportCell.SetValue(ActiveText);
        this.inactiveReportCell = new ReportCell();
        this.inactiveReportCell.SetValue(InactiveText);
        this.deletedReportCell = new ReportCell();
        this.deletedReportCell.SetValue(DeletedText);
    }

    // Returns correct cell depending on whether entity id delete, inactive or active.
    public ReportCell GetCell(TData entity)
    {
        return this.isDeleted(entity) ?
            this.deletedReportCell :
            this.isActive(entity) ?
                this.activeReportCell :
                this.inactiveReportCell;
    }
}
```

Now we have our cell provider, so let's apply it to some reports:

```c#
class Product
{
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}

Product[] products = new Product[]
{
    new Product() { Name = "Notebook" },
    new Product() { Name = "PC", IsDeleted = true },
    new Product() { Name = "iPhone", IsActive = false },
};

ReportSchemaBuilder<Product> productBuilder = new ReportSchemaBuilder<Product>();

productBuilder.AddColumn("Status", new StatusCellProvider<Product>(p => p.IsActive, p => p.IsDeleted));
productBuilder.AddColumn("Name", (Product p) => p.Name);

/*
|               Status |                 Name |
|----------------------|----------------------|
|                    ☑ |             Notebook |
|                    ☒ |                   PC |
|                    ☐ |               iPhone |
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

User[] users = new User[]
{
    new User() { Username = "admin" },
    new User() { Username = "user1", IsDeleted = true },
    new User() { Username = "user2", IsBlocked = true },
};

ReportSchemaBuilder<User> userBuilder = new ReportSchemaBuilder<User>();

userBuilder.AddColumn("User status", new StatusCellProvider<User>(p => !p.IsBlocked, p => p.IsDeleted));
userBuilder.AddColumn("Username", (User p) => p.Username);

/*
|          User status |             Username |
|----------------------|----------------------|
|                    ☑ |                admin |
|                    ☒ |                user1 |
|                    ☐ |                user2 |
*/
```

Having logic implemented in separate class allows changing status representation later easily.
