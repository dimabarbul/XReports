using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();

// Specify columns. They will become rows during building schema.
builder.AddColumn("Name", (Customer x) => x.Name);
builder.AddColumn("Phone", (Customer x) => x.Phone);
builder.AddColumn("Email", (Customer x) => x.Email);
builder.AddColumn("Age", (Customer x) => x.Age);

// Add complex header. For horizontal report header cells are at left.
builder.AddComplexHeader(0, "Contact Info", "Phone", "Email");

// Build schema.
IReportSchema<Customer> schema = builder.BuildHorizontalSchema(0);

IEnumerable<Customer> dataSource = new Customer[]
{
    new Customer { Name = "John Doe", Email = "john@example.com", Age = 23, Phone = "1111111111" },
    new Customer { Name = "Jane Doe", Email = "jane@example.com", Age = 22, Phone = "2222222222" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataSource);

new ConsoleWriter().Write(reportTable);

class Customer
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}
