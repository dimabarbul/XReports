using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();

// Specify columns. They will become rows during building schema.
builder.AddColumn(string.Empty, (Customer x) => x.Name.ToUpperInvariant());
builder.AddColumn("Email", (Customer x) => x.Email);
builder.AddColumn("Age", (Customer x) => x.Age);

// Build horizontal schema. The first column should become header row.
IReportSchema<Customer> schema = builder.BuildHorizontalSchema(1);

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23 },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22 },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

new SimpleConsoleWriter().Write(reportTable);

internal class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}
