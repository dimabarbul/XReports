using System.Data;
using System.Drawing;
using XReports.Benchmarks.Core.Models;
using XReports.Benchmarks.Core.ReportStructure.Enums;
using XReports.Benchmarks.Core.ReportStructure.Models;
using XReports.Benchmarks.Core.ReportStructure.Models.Properties;

namespace XReports.Benchmarks.Core.ReportStructure;

public static class ReportStructureProvider
{
    public static IEnumerable<ReportCellsSource<Person>> GetEntitiesCellsSources()
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

        yield return new TypedReportCellsSource<Person, string>("FirstName", e => e.FirstName, boldProperty);
        yield return new TypedReportCellsSource<Person, string>("LastName", e => e.LastName, boldProperty);
        yield return new TypedReportCellsSource<Person, string>("Email", e => e.Email, highlighted);
        yield return new TypedReportCellsSource<Person, decimal>("Score", e => e.Score, customFormatProperty, centerAlignment);
        yield return new TypedReportCellsSource<Person, string>("Account Number", e => e.AccountNumber, leftAlignment);
        yield return new TypedReportCellsSource<Person, string>("Btc Wallet", e => e.BtcAddress, leftAlignment);
        yield return new TypedReportCellsSource<Person, string>("Eth Wallet", e => e.EthAddress, leftAlignment);
        yield return new TypedReportCellsSource<Person, decimal>("Account #", e => e.AccountAmount, rightAlignment, accountAmountPrecisionProperty);
        yield return new TypedReportCellsSource<Person, decimal>("Btc #", e => e.BtcAmount, rightAlignment, cryptoAmountPrecisionProperty);
        yield return new TypedReportCellsSource<Person, decimal>("Eth #", e => e.EthAmount, rightAlignment, cryptoAmountPrecisionProperty);
        yield return new TypedReportCellsSource<Person, string>("Bio", e => e.Bio);
        yield return new TypedReportCellsSource<Person, DateTime>("DOB", e => e.DateOfBirth, dateOfBirthFormatProperty);
        yield return new TypedReportCellsSource<Person, string>("Company Name", e => e.CompanyName, centerAlignment);
        yield return new TypedReportCellsSource<Person, string>("Preferred Color", e => e.PreferredColor);
        yield return new TypedReportCellsSource<Person, string>("Avatar", e => e.Avatar);
        yield return new TypedReportCellsSource<Person, string>("Password", e => e.Password, centerAlignment);
        yield return new TypedReportCellsSource<Person, string>("Username", e => e.UserName);
        yield return new TypedReportCellsSource<Person, string>("Home Page", e => e.HomePage);
        yield return new TypedReportCellsSource<Person, string>("Last IP", e => e.LastIpAddress, rightAlignment);
        yield return new TypedReportCellsSource<Person, string>("User Agent", e => e.BrowserUserAgent);
        yield return new TypedReportCellsSource<Person, string>("Lorem Ipsum", e => e.Text, leftAlignment);
        yield return new TypedReportCellsSource<Person, DateTime>("Registered", e => e.RegisteredAt, dateTimeFormatProperty);
        yield return new TypedReportCellsSource<Person, DateTime>("Last Visited", e => e.LastVisitedAt, dateTimeFormatProperty);
        yield return new TypedReportCellsSource<Person, string>("Home Phone", e => e.HomePhone, rightAlignment);
        yield return new TypedReportCellsSource<Person, string>("Work Phone", e => e.WorkPhone, rightAlignment);
        yield return new TypedReportCellsSource<Person, string>("Locale", e => e.Locale);
        yield return new TypedReportCellsSource<Person, string>("Country", e => e.Address.Country, centerAlignment);
        yield return new TypedReportCellsSource<Person, string>("City", e => e.Address.City);
        yield return new TypedReportCellsSource<Person, string>("Zip", e => e.Address.ZipCode, rightAlignment);
        yield return new TypedReportCellsSource<Person, string>("Address", e => e.Address.StreetAddress1);
        yield return new TypedReportCellsSource<Person, string>("Second Address Line", e => e.Address.StreetAddress2);
        yield return new TypedReportCellsSource<Person, string>("Manufacturer", e => e.Vehicle.Manufacturer);
        yield return new TypedReportCellsSource<Person, string>("Model", e => e.Vehicle.Model);
        yield return new TypedReportCellsSource<Person, string>("Vin", e => e.Vehicle.Vin, highlighted);
        yield return new TypedReportCellsSource<Person, string>("Fuel Type", e => e.Vehicle.FuelType);
        yield return new TypedReportCellsSource<Person, string>("Type", e => e.Vehicle.Type);
    }

    public static IEnumerable<ReportCellsSource<IDataReader>> GetDataReaderCellsSources()
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

        yield return new TypedReportCellsSource<IDataReader, string>("FirstName", e => e.GetString(0), boldProperty);
        yield return new TypedReportCellsSource<IDataReader, string>("LastName", e => e.GetString(1), boldProperty);
        yield return new TypedReportCellsSource<IDataReader, string>("Email", e => e.GetString(2), highlighted);
        yield return new TypedReportCellsSource<IDataReader, decimal>("Score", e => e.GetDecimal(3), customFormatProperty, centerAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Account Number", e => e.GetString(4), leftAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Btc Wallet", e => e.GetString(5), leftAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Eth Wallet", e => e.GetString(6), leftAlignment);
        yield return new TypedReportCellsSource<IDataReader, decimal>("Account #", e => e.GetDecimal(7), rightAlignment, accountAmountPrecisionProperty);
        yield return new TypedReportCellsSource<IDataReader, decimal>("Btc #", e => e.GetDecimal(8), rightAlignment, cryptoAmountPrecisionProperty);
        yield return new TypedReportCellsSource<IDataReader, decimal>("Eth #", e => e.GetDecimal(9), rightAlignment, cryptoAmountPrecisionProperty);
        yield return new TypedReportCellsSource<IDataReader, string>("Bio", e => e.GetString(10));
        yield return new TypedReportCellsSource<IDataReader, DateTime>("DOB", e => e.GetDateTime(11), dateOfBirthFormatProperty);
        yield return new TypedReportCellsSource<IDataReader, string>("Company Name", e => e.GetString(12), centerAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Preferred Color", e => e.GetString(13));
        yield return new TypedReportCellsSource<IDataReader, string>("Avatar", e => e.GetString(14));
        yield return new TypedReportCellsSource<IDataReader, string>("Password", e => e.GetString(15), centerAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Username", e => e.GetString(16));
        yield return new TypedReportCellsSource<IDataReader, string>("Home Page", e => e.GetString(17));
        yield return new TypedReportCellsSource<IDataReader, string>("Last IP", e => e.GetString(18), rightAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("User Agent", e => e.GetString(19));
        yield return new TypedReportCellsSource<IDataReader, string>("Lorem Ipsum", e => e.GetString(20), leftAlignment);
        yield return new TypedReportCellsSource<IDataReader, DateTime>("Registered", e => e.GetDateTime(21), dateTimeFormatProperty);
        yield return new TypedReportCellsSource<IDataReader, DateTime>("Last Visited", e => e.GetDateTime(22), dateTimeFormatProperty);
        yield return new TypedReportCellsSource<IDataReader, string>("Home Phone", e => e.GetString(23), rightAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Work Phone", e => e.GetString(24), rightAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Locale", e => e.GetString(25));
        yield return new TypedReportCellsSource<IDataReader, string>("Country", e => e.GetString(26), centerAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("City", e => e.GetString(27));
        yield return new TypedReportCellsSource<IDataReader, string>("Zip", e => e.GetString(28), rightAlignment);
        yield return new TypedReportCellsSource<IDataReader, string>("Address", e => e.GetString(29));
        yield return new TypedReportCellsSource<IDataReader, string>("Second Address Line", e => e.GetString(30));
        yield return new TypedReportCellsSource<IDataReader, string>("Manufacturer", e => e.GetString(31));
        yield return new TypedReportCellsSource<IDataReader, string>("Model", e => e.GetString(32));
        yield return new TypedReportCellsSource<IDataReader, string>("Vin", e => e.GetString(33), highlighted);
        yield return new TypedReportCellsSource<IDataReader, string>("Fuel Type", e => e.GetString(34));
        yield return new TypedReportCellsSource<IDataReader, string>("Type", e => e.GetString(35));
    }

    public static IEnumerable<ReportCellsSourceProperty> GetGlobalProperties()
    {
        yield return new SameColumnFormatProperty();
    }
}
