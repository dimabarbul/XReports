using OfficeOpenXml;
using OfficeOpenXml.Style;
using XReports.Converter;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<User> builder = new ReportSchemaBuilder<User>();
ReportConverter<ExcelReportCell> converter = new ReportConverter<ExcelReportCell>();
User[] data =
{
    new User() { Name = "John Doe", Email = "johndoe@example.com" },
    new User() { Name = "Jane Doe", Email = "janedoe@example.com" },
};

builder.AddColumn("Name", (User user) => user.Name)
    .AddProperties(new IndentationProperty(), new SameColumnFormatProperty());
builder.AddColumn("E-mail", (User user) => user.Email);
IReportSchema<User> schema = builder.BuildVerticalSchema();
IReportTable<ReportCell> reportTable = schema.BuildReportTable(data);
IReportTable<ExcelReportCell> report = converter.Convert(reportTable);

IEpplusWriter writer = new EpplusWriter(new[]
{
    new ExcelIndentationPropertyFormatter(),
});
string excelFileName = Path.GetTempFileName() + ".xlsx";
writer.WriteToFile(report, excelFileName);

Console.WriteLine($"Excel report has been successfully written to {excelFileName}");

internal class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// Property marking cells that should be indented.
internal class IndentationProperty : IReportCellProperty { }

internal class ExcelIndentationPropertyFormatter : EpplusFormatter<IndentationProperty>
{
    protected override void Format(ExcelRange worksheetCell, ExcelReportCell cell, IndentationProperty property)
    {
        worksheetCell.Style.Indent = 1;
        worksheetCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
    }
}
