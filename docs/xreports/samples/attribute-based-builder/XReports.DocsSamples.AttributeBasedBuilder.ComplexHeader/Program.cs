using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

User[] users =
{
    new User()
    {
        Name = "John Doe",
        Email = "johndoe@example.com",
        Phone = "1234567890",
        Age = 21,
    },
    new User()
    {
        Name = "Jane Doe",
        Email = "janedoe@example.com",
        Phone = "1234567891",
        Age = 20,
    },
};

AttributeBasedBuilder builder = new AttributeBasedBuilder(Array.Empty<IAttributeHandler>());
IReportSchema<User> schema = builder.BuildSchema<User>();

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);
new ConsoleWriter().Write(reportTable);

// Add complex header using property names.
// The first parameter is the header row number. The lower the number, the
// higher the header row is.
[ComplexHeader(1, "User Info", nameof(Name), nameof(Email), true)]
// Add complex header using columns/rows titles.
[ComplexHeader(2, "Personal Info", "Full Name", "Age")]
// Add complex header using column/row order.
// Note that numbers here are column order values - the numbers
// used in ReportColumnAttribute, not columns/rows indexes.
[ComplexHeader(2, "Contact Info", 3, 4)]
internal class User
{
    [ReportColumn(1, "Full Name")]
    public string Name { get; set; }

    [ReportColumn(2)]
    public int Age { get; set; }

    [ReportColumn(3)]
    public string Phone { get; set; }

    [ReportColumn(4)]
    public string Email { get; set; }
}
