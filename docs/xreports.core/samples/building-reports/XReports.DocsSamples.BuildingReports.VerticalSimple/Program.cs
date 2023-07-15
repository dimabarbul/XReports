using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

// Can use dynamic if you don't know type of data.
ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();

// Specify columns.
builder.AddColumn("Name", (Customer x) => x.Name);
builder.AddColumn("Email", (Customer x) => x.Email);
builder.AddColumn("Age", (Customer x) => x.Age);

// Build schema.
IReportSchema<Customer> schema = builder.BuildVerticalSchema();

// Data source is IEnumerable which allows iterating over large
// collection without having to load it into memory.
IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23, },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22, },
};

// Report table contains header and body of the report.
IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

// SimpleConsoleWriter is an example class that writes report table data into console.
new SimpleConsoleWriter().Write(reportTable);

// Model describing report data source item.
internal class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}
