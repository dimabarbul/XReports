using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();
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
IReportSchema<Customer> schema = builder.BuildVerticalSchema();

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23, JobTitle = "Manager", Salary = 1500m },
    new Customer { Name = "David Doe", Email = "david@example.com", Age = 22, JobTitle = "Director", Salary = 2000m },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

new ConsoleWriter().Write(reportTable);

class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string JobTitle { get; set; }
    public decimal Salary { get; set; }
}
