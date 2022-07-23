using System.Diagnostics;
using System.Drawing;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XReports.DependencyInjection;
using XReports.Enums;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.BigReport;

internal static class Program
{
    public static async Task Main()
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(s =>
            {
                s
                    .AddStringCellWriter()
                    .AddHtmlStringWriter<HtmlStringWriter>()
                    .AddStreamCellWriter()
                    .AddHtmlStreamWriter<HtmlStreamWriter>()
                    .AddEpplusWriter<EpplusWriter>()
                    .AddReportConverter<HtmlReportCell>(
                        new AlignmentPropertyHtmlHandler(),
                        new BoldPropertyHtmlHandler(),
                        new ColorPropertyHtmlHandler(),
                        new DecimalPrecisionPropertyHtmlHandler(),
                        new PercentFormatPropertyHtmlHandler(),
                        new DateTimeFormatPropertyHtmlHandler(),
                        new CustomFormatPropertyHtmlHandler())
                    .AddReportConverter<ExcelReportCell>(
                        new AlignmentPropertyExcelHandler(),
                        new BoldPropertyExcelHandler(),
                        new ColorPropertyExcelHandler(),
                        new DecimalPrecisionPropertyExcelHandler(),
                        new PercentFormatPropertyExcelHandler(),
                        new DateTimeFormatPropertyExcelHandler(),
                        new CustomFormatPropertyExcelHandler());
            })
            .Build();

        ReportBuilder builder = new(
            1_00_000,
            host.Services.GetRequiredService<IReportConverter<HtmlReportCell>>(),
            host.Services.GetRequiredService<IEpplusWriter>(),
            host.Services.GetRequiredService<IReportConverter<ExcelReportCell>>(),
            host.Services.GetRequiredService<IHtmlStringWriter>(),
            host.Services.GetRequiredService<IHtmlStreamWriter>());

        Stopwatch sw = Stopwatch.StartNew();
        builder.ToString();
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
    }
}


public class ReportBuilder
{
    private readonly int recordsCount;
    private readonly IReportConverter<HtmlReportCell> htmlConverter;
    private readonly IEpplusWriter excelWriter;
    private readonly IReportConverter<ExcelReportCell> excelConverter;
    private readonly IHtmlStringWriter htmlStringWriter;
    private readonly IHtmlStreamWriter htmlStreamWriter;

    public ReportBuilder(int recordsCount, IReportConverter<HtmlReportCell> htmlConverter, IEpplusWriter excelWriter, IReportConverter<ExcelReportCell> excelConverter, IHtmlStringWriter htmlStringWriter, IHtmlStreamWriter htmlStreamWriter)
    {
        this.recordsCount = recordsCount;
        this.htmlConverter = htmlConverter;
        this.excelWriter = excelWriter;
        this.excelConverter = excelConverter;
        this.htmlStringWriter = htmlStringWriter;
        this.htmlStreamWriter = htmlStreamWriter;
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

    public string ToString()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        return this.WriteReportToString(htmlReportTable);
    }

    public Task ToStreamAsync(Stream stream)
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        return this.WriteReportToStreamAsync(htmlReportTable, stream);
    }

    public Task ToStreamAsync(StreamWriter streamWriter)
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        return this.WriteReportToStreamAsync(htmlReportTable, streamWriter);
    }

    public async Task ToExcelFileAsStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        Stream excelStream = this.WriteExcelReportToStream(excelReportTable);

        FileStream fileStream = File.Create("/tmp/report.xlsx");

        await excelStream.CopyToAsync(fileStream);
        fileStream.Close();
    }

    public void ToExcelFileAsFileStream()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        FileStream fileStream = File.Create("/tmp/report.xlsx");
        this.WriteExcelReportToStream(excelReportTable, fileStream);

        fileStream.Close();
    }

    public void ToExcelFile()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.WriteExcelReportToFile(excelReportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        return this.excelWriter.WriteToStream(reportTable);
    }

    private void WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable, Stream stream)
    {
        this.excelWriter.WriteToStream(reportTable, stream);
    }

    private void WriteExcelReportToFile(IReportTable<ExcelReportCell> reportTable)
    {
        const string fileName = "/tmp/report.xlsx";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        this.excelWriter.WriteToFile(reportTable, fileName);
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

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
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

    private string WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
    {
        return this.htmlStringWriter.WriteToString(htmlReportTable);
    }

    private Task WriteReportToStreamAsync(IReportTable<HtmlReportCell> htmlReportTable, Stream stream)
    {
        return this.htmlStreamWriter.WriteAsync(htmlReportTable, stream);
    }

    private Task WriteReportToStreamAsync(IReportTable<HtmlReportCell> htmlReportTable, StreamWriter streamWriter)
    {
        return this.htmlStreamWriter.WriteAsync(htmlReportTable, streamWriter);
    }

    private IEnumerable<Person> GetData()
    {
        int luckyGuyIndex = new Random().Next(3, this.recordsCount - 1);

        List<Person> data = new Faker<Person>()
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Email, f => f.Internet.Email())
            .RuleFor(e => e.AccountNumber, f => f.Finance.Account())
            .RuleFor(e => e.BtcAddress, f => f.Finance.BitcoinAddress())
            .RuleFor(e => e.EthAddress, f => f.Finance.EthereumAddress())
            .RuleFor(e => e.AccountAmount, f => f.Random.Decimal(0m, 1000m))
            .RuleFor(e => e.BtcAmount, f => f.Random.Decimal())
            .RuleFor(e => e.EthAmount, f => f.Random.Decimal())
            .RuleFor(e => e.Bio, f => f.Random.Words(100))
            .RuleFor(e => e.DateOfBirth, f => f.Date.Past(60))
            .RuleFor(e => e.CompanyName, f => f.Company.CompanyName())
            .RuleFor(e => e.PreferredColor, f => f.Internet.Color())
            .RuleFor(e => e.Avatar, f => f.Internet.Avatar())
            .RuleFor(e => e.Password, f => f.Internet.Password())
            .RuleFor(e => e.UserName, f => f.Internet.UserName())
            .RuleFor(e => e.HomePage, f => f.Internet.Url())
            .RuleFor(e => e.LastIpAddress, f => f.Internet.IpAddress().ToString())
            .RuleFor(e => e.BrowserUserAgent, f => f.Internet.UserAgent())
            .RuleFor(e => e.Text, f => f.Lorem.Text())
            .RuleFor(e => e.RegisteredAt, f => f.Date.Past())
            .RuleFor(e => e.LastVisitedAt, f => f.Date.Past())
            .RuleFor(e => e.HomePhone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.WorkPhone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.Locale, f => f.System.Locale)
            .RuleFor(e => e.Vehicle, f => new Vehicle
            {
                Manufacturer = f.Vehicle.Manufacturer(),
                Model = f.Vehicle.Model(),
                Vin = f.Vehicle.Vin(),
                FuelType = f.Vehicle.Fuel(),
                Type = f.Vehicle.Type(),
            })
            .RuleFor(e => e.Address, f => new Address
            {
                Country = f.Address.Country(),
                City = f.Address.City(),
                ZipCode = f.Address.ZipCode(),
                StreetAddress1 = f.Address.StreetAddress(),
                StreetAddress2 = f.Address.SecondaryAddress(),
            })
            .RuleFor(e => e.Score, f => f.IndexFaker % luckyGuyIndex == 0 ? 100m : f.Random.Decimal(80, 100))
            .Generate(this.recordsCount / 20);

        IEnumerable<Person> dataX5 = data
            .Concat(data)
            .Concat(data)
            .Concat(data)
            .Concat(data);

        return dataX5
            .Concat(dataX5)
            .Concat(dataX5)
            .Concat(dataX5);
    }

    private class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Score { get; set; }

        public string AccountNumber { get; set; }
        public string BtcAddress { get; set; }
        public string EthAddress { get; set; }
        public decimal AccountAmount { get; set; }
        public decimal BtcAmount { get; set; }
        public decimal EthAmount { get; set; }

        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Address Address { get; set; }
        public string CompanyName { get; set; }
        public string PreferredColor { get; set; }
        public string Avatar { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string HomePage { get; set; }
        public object LastIpAddress { get; set; }
        public string BrowserUserAgent { get; set; }
        public string Text { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime LastVisitedAt { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Locale { get; set; }
        public Vehicle Vehicle { get; set; }
    }

    private class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
    }

    private class Vehicle
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string FuelType { get; set; }
        public string Type { get; set; }
    }
}


public class CustomFormatProperty : ReportCellProperty
{
}

public class CustomFormatPropertyHtmlHandler : PropertyHandler<CustomFormatProperty, HtmlReportCell>
{
    protected override void HandleProperty(CustomFormatProperty property, HtmlReportCell cell)
    {
        decimal value = cell.GetValue<decimal>();
        string format = value == 100m ? "F0" : "F2";

        cell.SetValue(value.ToString(format));
    }
}

public class CustomFormatPropertyExcelHandler : PropertyHandler<CustomFormatProperty, ExcelReportCell>
{
    protected override void HandleProperty(CustomFormatProperty property, ExcelReportCell cell)
    {
        cell.NumberFormat = "[=100]0;[<100]0.00";
    }
}
