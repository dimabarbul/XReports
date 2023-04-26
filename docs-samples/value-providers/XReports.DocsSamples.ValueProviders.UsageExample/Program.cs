using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders.ValueProviders;
using XReports.Table;

ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();

// AddColumn can accept instance of IValueProvider.
builder.AddColumn("#", new SequentialNumberValueProvider());
builder.AddColumn("Name", (Customer c) => c.Name);
builder.AddColumn("Email", (Customer c) => c.Email);

IReportSchema<Customer> schema = builder.BuildVerticalSchema();

Customer[] customers = new Customer[]
{
    new Customer() { Name = "John Doe", Email = "john@example.com" },
    new Customer() { Name = "Jane Doe", Email = "jane@example.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

new SimpleConsoleWriter().Write(reportTable);

class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
}
