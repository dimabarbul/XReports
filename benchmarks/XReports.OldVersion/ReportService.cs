using System.Data;
using System.Drawing;
using XReports.BenchmarksCore.Interfaces;
using XReports.BenchmarksCore.Models;
using XReports.Enums;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.OldVersion.XReportsProperties;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.OldVersion;

public class ReportService : IReportService
{
    private readonly IEnumerable<Person> data;
    private readonly DataTable dataTable;
    private readonly IReportConverter<HtmlReportCell> htmlConverter;
    private readonly IEpplusWriter excelWriter;
    private readonly IReportConverter<ExcelReportCell> excelConverter;
    private readonly IStringWriter stringWriter;

    public ReportService(IEnumerable<Person> data, DataTable dataTable)
    {
        this.data = data;
        this.dataTable = dataTable;

        this.excelWriter = new EpplusWriter();
        this.stringWriter = new Writers.StringWriter(new StringCellWriter());
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

    public Task VerticalFromEntitiesHtmlEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
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

    public Task VerticalFromEntitiesExcelEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        foreach (ReportTableProperty _ in excelReportTable.Properties)
        {
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.HeaderRows)
        {
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> VerticalFromEntitiesHtmlToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return this.stringWriter.WriteToStringAsync(htmlReportTable);
    }

    public Task VerticalFromEntitiesHtmlToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return this.stringWriter.WriteToFileAsync(htmlReportTable, fileName);
    }

    public Task<Stream> VerticalFromEntitiesExcelToStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        return Task.FromResult(this.excelWriter.WriteToStream(excelReportTable));
    }

    public Task VerticalFromEntitiesExcelToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromEntities();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.excelWriter.WriteToFile(excelReportTable, fileName);

        return Task.CompletedTask;
    }

    public Task VerticalFromDataReaderHtmlEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
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

    public Task VerticalFromDataReaderExcelEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        foreach (ReportTableProperty _ in excelReportTable.Properties)
        {
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.HeaderRows)
        {
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> VerticalFromDataReaderHtmlToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return this.stringWriter.WriteToStringAsync(htmlReportTable);
    }

    public Task VerticalFromDataReaderHtmlToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return this.stringWriter.WriteToFileAsync(htmlReportTable, fileName);
    }

    public Task<Stream> VerticalFromDataReaderExcelToStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        return Task.FromResult(this.excelWriter.WriteToStream(excelReportTable));
    }

    public Task VerticalFromDataReaderExcelToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildVerticalReportFromDataReader();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.excelWriter.WriteToFile(excelReportTable, fileName);

        return Task.CompletedTask;
    }

    public Task HorizontalHtmlEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
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

    public Task HorizontalExcelEnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        foreach (ReportTableProperty _ in excelReportTable.Properties)
        {
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.HeaderRows)
        {
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<ExcelReportCell> row in excelReportTable.Rows)
        {
            foreach (ExcelReportCell cell in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public Task<string> HorizontalHtmlToStringAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return this.stringWriter.WriteToStringAsync(htmlReportTable);
    }

    public Task HorizontalHtmlToFileAsync(string fileName)
    {
        IReportTable<ReportCell> reportTable = this.BuildHorizontalReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        return this.stringWriter.WriteToFileAsync(htmlReportTable, fileName);
    }

    private IReportTable<ReportCell> BuildVerticalReportFromEntities()
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

    private IReportTable<ReportCell> BuildVerticalReportFromDataReader()
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
        VerticalReportSchemaBuilder<IDataReader> reportBuilder = new();
        reportBuilder.AddColumn("FirstName", e => e.GetString(0))
            .AddProperties(boldProperty);
        reportBuilder.AddColumn("LastName", e => e.GetString(1))
            .AddProperties(boldProperty);
        reportBuilder.AddColumn("Email", e => e.GetString(2))
            .AddProperties(highlighted);
        reportBuilder.AddColumn("Score", e => e.GetDecimal(3))
            .AddProperties(customFormatProperty, centerAlignment);
        reportBuilder.AddColumn("Account Number", e => e.GetString(4))
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Btc Wallet", e => e.GetString(5))
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Eth Wallet", e => e.GetString(6))
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Account #", e => e.GetDecimal(7))
            .AddProperties(rightAlignment, accountAmountPrecisionProperty);
        reportBuilder.AddColumn("Btc #", e => e.GetDecimal(8))
            .AddProperties(rightAlignment, cryptoAmountPrecisionProperty);
        reportBuilder.AddColumn("Eth #", e => e.GetDecimal(9))
            .AddProperties(rightAlignment, cryptoAmountPrecisionProperty);
        reportBuilder.AddColumn("Bio", e => e.GetString(10))
            .AddProperties();
        reportBuilder.AddColumn("DOB", e => e.GetDateTime(11))
            .AddProperties(dateOfBirthFormatProperty);
        reportBuilder.AddColumn("Company Name", e => e.GetString(12))
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("Preferred Color", e => e.GetString(13));
        reportBuilder.AddColumn("Avatar", e => e.GetString(14))
            .AddProperties();
        reportBuilder.AddColumn("Password", e => e.GetString(15))
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("Username", e => e.GetString(16))
            .AddProperties();
        reportBuilder.AddColumn("Home Page", e => e.GetString(17))
            .AddProperties();
        reportBuilder.AddColumn("Last IP", e => e.GetString(18))
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("User Agent", e => e.GetString(19))
            .AddProperties();
        reportBuilder.AddColumn("Lorem Ipsum", e => e.GetString(20))
            .AddProperties(leftAlignment);
        reportBuilder.AddColumn("Registered", e => e.GetDateTime(21))
            .AddProperties(dateTimeFormatProperty);
        reportBuilder.AddColumn("Last Visited", e => e.GetDateTime(22))
            .AddProperties(dateTimeFormatProperty);
        reportBuilder.AddColumn("Home Phone", e => e.GetString(23))
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("Work Phone", e => e.GetString(24))
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("Locale", e => e.GetString(25))
            .AddProperties();
        reportBuilder.AddColumn("Country", e => e.GetString(26))
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("City", e => e.GetString(27))
            .AddProperties();
        reportBuilder.AddColumn("Zip", e => e.GetString(28))
            .AddProperties(rightAlignment);
        reportBuilder.AddColumn("Address", e => e.GetString(29))
            .AddProperties();
        reportBuilder.AddColumn("Second Address Line", e => e.GetString(30))
            .AddProperties();
        reportBuilder.AddColumn("Manufacturer", e => e.GetString(31))
            .AddProperties();
        reportBuilder.AddColumn("Model", e => e.GetString(32))
            .AddProperties();
        reportBuilder.AddColumn("Vin", e => e.GetString(33))
            .AddProperties(highlighted);
        reportBuilder.AddColumn("Fuel Type", e => e.GetString(34))
            .AddProperties();
        reportBuilder.AddColumn("Type", e => e.GetString(35))
            .AddProperties();

        reportBuilder.AddGlobalProperties(new SameColumnFormatProperty());

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(new DataTableReader(this.dataTable));

        return reportTable;
    }

    private IReportTable<ReportCell> BuildHorizontalReport()
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
        HorizontalReportSchemaBuilder<Person> reportBuilder = new();
        reportBuilder.AddRow("FirstName", e => e.FirstName)
            .AddProperties(boldProperty);
        reportBuilder.AddRow("LastName", e => e.LastName)
            .AddProperties(boldProperty);
        reportBuilder.AddRow("Email", e => e.Email)
            .AddProperties(highlighted);
        reportBuilder.AddRow("Score", e => e.Score)
            .AddProperties(customFormatProperty, centerAlignment);
        reportBuilder.AddRow("Account Number", e => e.AccountNumber)
            .AddProperties(leftAlignment);
        reportBuilder.AddRow("Btc Wallet", e => e.BtcAddress)
            .AddProperties(leftAlignment);
        reportBuilder.AddRow("Eth Wallet", e => e.EthAddress)
            .AddProperties(leftAlignment);
        reportBuilder.AddRow("Account #", e => e.AccountAmount)
            .AddProperties(rightAlignment, accountAmountPrecisionProperty);
        reportBuilder.AddRow("Btc #", e => e.BtcAmount)
            .AddProperties(rightAlignment, cryptoAmountPrecisionProperty);
        reportBuilder.AddRow("Eth #", e => e.EthAmount)
            .AddProperties(rightAlignment, cryptoAmountPrecisionProperty);
        reportBuilder.AddRow("Bio", e => e.Bio)
            .AddProperties();
        reportBuilder.AddRow("DOB", e => e.DateOfBirth)
            .AddProperties(dateOfBirthFormatProperty);
        reportBuilder.AddRow("Company Name", e => e.CompanyName)
            .AddProperties(centerAlignment);
        reportBuilder.AddRow("Preferred Color", e => e.PreferredColor);
        reportBuilder.AddRow("Avatar", e => e.Avatar)
            .AddProperties();
        reportBuilder.AddRow("Password", e => e.Password)
            .AddProperties(centerAlignment);
        reportBuilder.AddRow("Username", e => e.UserName)
            .AddProperties();
        reportBuilder.AddRow("Home Page", e => e.HomePage)
            .AddProperties();
        reportBuilder.AddRow("Last IP", e => e.LastIpAddress)
            .AddProperties(rightAlignment);
        reportBuilder.AddRow("User Agent", e => e.BrowserUserAgent)
            .AddProperties();
        reportBuilder.AddRow("Lorem Ipsum", e => e.Text)
            .AddProperties(leftAlignment);
        reportBuilder.AddRow("Registered", e => e.RegisteredAt)
            .AddProperties(dateTimeFormatProperty);
        reportBuilder.AddRow("Last Visited", e => e.LastVisitedAt)
            .AddProperties(dateTimeFormatProperty);
        reportBuilder.AddRow("Home Phone", e => e.HomePhone)
            .AddProperties(rightAlignment);
        reportBuilder.AddRow("Work Phone", e => e.WorkPhone)
            .AddProperties(rightAlignment);
        reportBuilder.AddRow("Locale", e => e.Locale)
            .AddProperties();
        reportBuilder.AddRow("Country", e => e.Address.Country)
            .AddProperties(centerAlignment);
        reportBuilder.AddRow("City", e => e.Address.City)
            .AddProperties();
        reportBuilder.AddRow("Zip", e => e.Address.ZipCode)
            .AddProperties(rightAlignment);
        reportBuilder.AddRow("Address", e => e.Address.StreetAddress1)
            .AddProperties();
        reportBuilder.AddRow("Second Address Line", e => e.Address.StreetAddress2)
            .AddProperties();
        reportBuilder.AddRow("Manufacturer", e => e.Vehicle.Manufacturer)
            .AddProperties();
        reportBuilder.AddRow("Model", e => e.Vehicle.Model)
            .AddProperties();
        reportBuilder.AddRow("Vin", e => e.Vehicle.Vin)
            .AddProperties(highlighted);
        reportBuilder.AddRow("Fuel Type", e => e.Vehicle.FuelType)
            .AddProperties();
        reportBuilder.AddRow("Type", e => e.Vehicle.Type)
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
