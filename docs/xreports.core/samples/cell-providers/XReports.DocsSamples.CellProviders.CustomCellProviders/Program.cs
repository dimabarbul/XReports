using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

IReportTable<ReportCell> reportTable;
ConsoleWriter writer = new ConsoleWriter();

#region Product report

ReportSchemaBuilder<Product> productBuilder = new ReportSchemaBuilder<Product>();

productBuilder.AddColumn("Status", new StatusCellProvider<Product>(p => p.IsActive, p => p.IsDeleted));
productBuilder.AddColumn("Name", (Product p) => p.Name);

IReportSchema<Product> productSchema = productBuilder.BuildVerticalSchema();

Product[] products = new Product[]
{
    new Product() { Name = "Notebook" },
    new Product() { Name = "PC", IsDeleted = true },
    new Product() { Name = "iPhone", IsActive = false },
};

reportTable = productSchema.BuildReportTable(products);

Console.WriteLine("Products");
writer.Write(reportTable);
Console.WriteLine();
Console.WriteLine();

#endregion

#region User report

ReportSchemaBuilder<User> userBuilder = new ReportSchemaBuilder<User>();

userBuilder.AddColumn("User status", new StatusCellProvider<User>(p => !p.IsBlocked, p => p.IsDeleted));
userBuilder.AddColumn("Username", (User p) => p.Username);

IReportSchema<User> userSchema = userBuilder.BuildVerticalSchema();

User[] users = new User[]
{
    new User() { Username = "admin" },
    new User() { Username = "user1", IsDeleted = true },
    new User() { Username = "user2", IsBlocked = true },
};

reportTable = userSchema.BuildReportTable(users);
Console.WriteLine("Users");
writer.Write(reportTable);
Console.WriteLine();
Console.WriteLine();


#endregion

internal class Product
{
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}

internal class User
{
    public string Username { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsDeleted { get; set; }
}

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
