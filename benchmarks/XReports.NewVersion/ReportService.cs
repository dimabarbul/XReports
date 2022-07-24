using System.Drawing;
using XReports.BenchmarksCore.Interfaces;
using XReports.BenchmarksCore.Models;
using XReports.Enums;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.NewVersion.XReportsProperties;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.NewVersion;

public class ReportService : IReportService
{
    private readonly IEnumerable<Person> data;
    private readonly IReportConverter<HtmlReportCell> htmlConverter;
    private readonly IEpplusWriter excelWriter;
    private readonly IReportConverter<ExcelReportCell> excelConverter;
    private readonly IHtmlStringWriter htmlStringWriter;
    private readonly IHtmlStreamWriter htmlStreamWriter;

    public ReportService(IEnumerable<Person> data)
    {
        this.data = data;
        this.htmlStreamWriter = new HtmlStreamWriter(new HtmlStreamCellWriter());
        this.excelWriter = new EpplusWriter();
        this.htmlStringWriter = new HtmlStringWriter(new HtmlStringCellWriter());
        this.htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
        {
            new AlignmentPropertyHtmlHandler(),
            new BoldPropertyHtmlHandler(),
            new ColorPropertyHtmlHandler(),
            new DecimalPrecisionPropertyHtmlHandler(),
            new PercentFormatPropertyHtmlHandler(),
            new DateTimeFormatPropertyHtmlHandler(),
            new CustomFormatPropertyHtmlHandler()
        });
        this.excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
        {
            new AlignmentPropertyExcelHandler(),
            new BoldPropertyExcelHandler(),
            new ColorPropertyExcelHandler(),
            new DecimalPrecisionPropertyExcelHandler(),
            new PercentFormatPropertyExcelHandler(),
            new DateTimeFormatPropertyExcelHandler(),
            new CustomFormatPropertyExcelHandler()
        });
    }

    public Task EnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        foreach (ReportTableProperty _ in htmlReportTable.Properties)
        {
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.HeaderRows)
        {
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> ToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return Task.FromResult(this.htmlStringWriter.WriteToString(htmlReportTable));
    }

    public async Task ToHtmlFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        await using Stream fileStream = File.Create(fileName);

        await this.htmlStreamWriter.WriteAsync(htmlReportTable, fileStream);
    }

    public Task<Stream> ToExcelStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        return Task.FromResult(this.excelWriter.WriteToStream(excelReportTable));
    }

    public Task ToExcelFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.excelWriter.WriteToFile(excelReportTable, fileName);

        return Task.CompletedTask;
    }

    private IReportTable<ReportCell> BuildReport()
    {
        CustomFormatProperty customFormatProperty = new();
        BoldProperty boldProperty = new();
        AlignmentProperty leftAlignment = new(Alignment.Left);
        AlignmentProperty rightAlignment = new(Alignment.Right);
        AlignmentProperty centerAlignment = new(Alignment.Center);
        DateTimeFormatProperty dateOfBirthFormatProperty = new("MMM d, yyyy");
        DateTimeFormatProperty dateTimeFormatProperty = new("yyyy/MM/dd HH:mm:ss");
        DecimalPrecisionProperty accountAmountPrecisionProperty = new(2);
        DecimalPrecisionProperty cryptoAmountPrecisionProperty = new(8);

        ColorProperty highlighted = new(Color.Blue);
        VerticalReportSchemaBuilder<Person> reportBuilder = new();
        reportBuilder.AddColumn("FirstName", e => e.FirstName)
            .AddProperties(boldProperty);
        reportBuilder.AddColumn("LastName", e => e.LastName)
            .AddProperties(boldProperty);
        reportBuilder.AddColumn("Email", e => e.Email)
            .AddProperties(highlighted);
        reportBuilder.AddColumn("Score", e => e.Score)
            .AddProperties(customFormatProperty, centerAlignment);
        reportBuilder.AddColumn("Account Number", e => e.AccountNumber)
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Btc Wallet", e => e.BtcAddress)
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Eth Wallet", e => e.EthAddress)
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Account #", e => e.AccountAmount)
            .AddProperties(rightAlignment, accountAmountPrecisionProperty);
        reportBuilder.AddColumn("Btc #", e => e.BtcAmount)
            .AddProperties(rightAlignment, cryptoAmountPrecisionProperty);
        reportBuilder.AddColumn("Eth #", e => e.EthAmount)
            .AddProperties(rightAlignment, cryptoAmountPrecisionProperty);
        reportBuilder.AddColumn("Bio", e => e.Bio)
            .AddProperties();
        reportBuilder.AddColumn("DOB", e => e.DateOfBirth)
            .AddProperties(dateOfBirthFormatProperty);
        reportBuilder.AddColumn("Company Name", e => e.CompanyName)
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("Preferred Color", e => e.PreferredColor);
        reportBuilder.AddColumn("Avatar", e => e.Avatar)
            .AddProperties();
        reportBuilder.AddColumn("Password", e => e.Password)
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("Username", e => e.UserName)
            .AddProperties();
        reportBuilder.AddColumn("Home Page", e => e.HomePage)
            .AddProperties();
        reportBuilder.AddColumn("Last IP", e => e.LastIpAddress)
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("User Agent", e => e.BrowserUserAgent)
            .AddProperties();
        reportBuilder.AddColumn("Lorem Ipsum", e => e.Text)
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Registered", e => e.RegisteredAt)
            .AddProperties(dateTimeFormatProperty);
        reportBuilder.AddColumn("Last Visited", e => e.LastVisitedAt)
            .AddProperties(dateTimeFormatProperty);
        reportBuilder.AddColumn("Home Phone", e => e.HomePhone)
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("Work Phone", e => e.WorkPhone)
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("Locale", e => e.Locale)
            .AddProperties();
        reportBuilder.AddColumn("Country", e => e.Address.Country)
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("City", e => e.Address.City)
            .AddProperties();
        reportBuilder.AddColumn("Zip", e => e.Address.ZipCode)
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("Address", e => e.Address.StreetAddress1)
            .AddProperties();
        reportBuilder.AddColumn("Second Address Line", e => e.Address.StreetAddress2)
            .AddProperties();
        reportBuilder.AddColumn("Manufacturer", e => e.Vehicle.Manufacturer)
            .AddProperties();
        reportBuilder.AddColumn("Model", e => e.Vehicle.Model)
            .AddProperties();
        reportBuilder.AddColumn("Vin", e => e.Vehicle.Vin)
            .AddProperties(highlighted);
        reportBuilder.AddColumn("Fuel Type", e => e.Vehicle.FuelType)
            .AddProperties();
        reportBuilder.AddColumn("Type", e => e.Vehicle.Type)
            .AddProperties();

        reportBuilder.AddGlobalProperties(new SameColumnFormatProperty());

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.data);
        return reportTable;
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        return this.htmlConverter.Convert(reportTable);
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        return this.excelConverter.Convert(reportTable);
    }
}
