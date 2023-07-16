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
    .AddProperties(new AlignmentProperty(Alignment.Center));
builder.AddColumn("E-mail", (User user) => user.Email);
IReportSchema<User> schema = builder.BuildVerticalSchema();
IReportTable<ReportCell> reportTable = schema.BuildReportTable(data);
IReportTable<ExcelReportCell> report = converter.Convert(reportTable);

IEpplusWriter writer = new MyEpplusWriter();
string excelFileName = Path.GetTempFileName() + ".xlsx";
writer.WriteToFile(report, excelFileName);

Console.WriteLine($"Excel report has been successfully written to {excelFileName}");

internal class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

internal class MyEpplusWriter : EpplusWriter
{
    protected override void PostCreate(ExcelWorksheet worksheet, ExcelAddress headerAddress, ExcelAddress bodyAddress)
    {
        base.PostCreate(worksheet, headerAddress, bodyAddress);

        worksheet.Cells[headerAddress.Address].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        worksheet.Cells[bodyAddress.Address].Style.Border.BorderAround(ExcelBorderStyle.Thin);
    }
}
