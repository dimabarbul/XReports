using System.Drawing;
using XReports.BenchmarksCore.ReportStructure.Enums;
using XReports.BenchmarksCore.ReportStructure.Models;
using XReports.BenchmarksCore.ReportStructure.Models.Properties;

namespace XReports.BenchmarksCore.ReportStructure;

public class ReportStructureProvider
{
    public IEnumerable<BaseReportCellsSourceFromEntities> GetEntitiesCellsSources()
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

        yield return new ReportCellsSourceFromEntities<string>("FirstName", e => e.FirstName, boldProperty);
        yield return new ReportCellsSourceFromEntities<string>("LastName", e => e.LastName, boldProperty);
        yield return new ReportCellsSourceFromEntities<string>("Email", e => e.Email, highlighted);
        yield return new ReportCellsSourceFromEntities<decimal>("Score", e => e.Score, customFormatProperty, centerAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Account Number", e => e.AccountNumber, leftAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Btc Wallet", e => e.BtcAddress, leftAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Eth Wallet", e => e.EthAddress, leftAlignment);
        yield return new ReportCellsSourceFromEntities<decimal>("Account #", e => e.AccountAmount, rightAlignment, accountAmountPrecisionProperty);
        yield return new ReportCellsSourceFromEntities<decimal>("Btc #", e => e.BtcAmount, rightAlignment, cryptoAmountPrecisionProperty);
        yield return new ReportCellsSourceFromEntities<decimal>("Eth #", e => e.EthAmount, rightAlignment, cryptoAmountPrecisionProperty);
        yield return new ReportCellsSourceFromEntities<string>("Bio", e => e.Bio);
        yield return new ReportCellsSourceFromEntities<DateTime>("DOB", e => e.DateOfBirth, dateOfBirthFormatProperty);
        yield return new ReportCellsSourceFromEntities<string>("Company Name", e => e.CompanyName, centerAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Preferred Color", e => e.PreferredColor);
        yield return new ReportCellsSourceFromEntities<string>("Avatar", e => e.Avatar);
        yield return new ReportCellsSourceFromEntities<string>("Password", e => e.Password, centerAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Username", e => e.UserName);
        yield return new ReportCellsSourceFromEntities<string>("Home Page", e => e.HomePage);
        yield return new ReportCellsSourceFromEntities<string>("Last IP", e => e.LastIpAddress, rightAlignment);
        yield return new ReportCellsSourceFromEntities<string>("User Agent", e => e.BrowserUserAgent);
        yield return new ReportCellsSourceFromEntities<string>("Lorem Ipsum", e => e.Text, leftAlignment);
        yield return new ReportCellsSourceFromEntities<DateTime>("Registered", e => e.RegisteredAt, dateTimeFormatProperty);
        yield return new ReportCellsSourceFromEntities<DateTime>("Last Visited", e => e.LastVisitedAt, dateTimeFormatProperty);
        yield return new ReportCellsSourceFromEntities<string>("Home Phone", e => e.HomePhone, rightAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Work Phone", e => e.WorkPhone, rightAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Locale", e => e.Locale);
        yield return new ReportCellsSourceFromEntities<string>("Country", e => e.Address.Country, centerAlignment);
        yield return new ReportCellsSourceFromEntities<string>("City", e => e.Address.City);
        yield return new ReportCellsSourceFromEntities<string>("Zip", e => e.Address.ZipCode, rightAlignment);
        yield return new ReportCellsSourceFromEntities<string>("Address", e => e.Address.StreetAddress1);
        yield return new ReportCellsSourceFromEntities<string>("Second Address Line", e => e.Address.StreetAddress2);
        yield return new ReportCellsSourceFromEntities<string>("Manufacturer", e => e.Vehicle.Manufacturer);
        yield return new ReportCellsSourceFromEntities<string>("Model", e => e.Vehicle.Model);
        yield return new ReportCellsSourceFromEntities<string>("Vin", e => e.Vehicle.Vin, highlighted);
        yield return new ReportCellsSourceFromEntities<string>("Fuel Type", e => e.Vehicle.FuelType);
        yield return new ReportCellsSourceFromEntities<string>("Type", e => e.Vehicle.Type);
    }

    public IEnumerable<ReportCellsSourceFromDataReader> GetDataReaderCellsSources()
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

        yield return new ReportCellsSourceFromDataReader("FirstName", e => e.GetString(0), boldProperty);
        yield return new ReportCellsSourceFromDataReader("LastName", e => e.GetString(1), boldProperty);
        yield return new ReportCellsSourceFromDataReader("Email", e => e.GetString(2), highlighted);
        yield return new ReportCellsSourceFromDataReader("Score", e => e.GetDecimal(3), customFormatProperty, centerAlignment);
        yield return new ReportCellsSourceFromDataReader("Account Number", e => e.GetString(4), leftAlignment);
        yield return new ReportCellsSourceFromDataReader("Btc Wallet", e => e.GetString(5), leftAlignment);
        yield return new ReportCellsSourceFromDataReader("Eth Wallet", e => e.GetString(6), leftAlignment);
        yield return new ReportCellsSourceFromDataReader("Account #", e => e.GetDecimal(7), rightAlignment, accountAmountPrecisionProperty);
        yield return new ReportCellsSourceFromDataReader("Btc #", e => e.GetDecimal(8), rightAlignment, cryptoAmountPrecisionProperty);
        yield return new ReportCellsSourceFromDataReader("Eth #", e => e.GetDecimal(9), rightAlignment, cryptoAmountPrecisionProperty);
        yield return new ReportCellsSourceFromDataReader("Bio", e => e.GetString(10));
        yield return new ReportCellsSourceFromDataReader("DOB", e => e.GetDateTime(11), dateOfBirthFormatProperty);
        yield return new ReportCellsSourceFromDataReader("Company Name", e => e.GetString(12), centerAlignment);
        yield return new ReportCellsSourceFromDataReader("Preferred Color", e => e.GetString(13));
        yield return new ReportCellsSourceFromDataReader("Avatar", e => e.GetString(14));
        yield return new ReportCellsSourceFromDataReader("Password", e => e.GetString(15), centerAlignment);
        yield return new ReportCellsSourceFromDataReader("Username", e => e.GetString(16));
        yield return new ReportCellsSourceFromDataReader("Home Page", e => e.GetString(17));
        yield return new ReportCellsSourceFromDataReader("Last IP", e => e.GetString(18), rightAlignment);
        yield return new ReportCellsSourceFromDataReader("User Agent", e => e.GetString(19));
        yield return new ReportCellsSourceFromDataReader("Lorem Ipsum", e => e.GetString(20), leftAlignment);
        yield return new ReportCellsSourceFromDataReader("Registered", e => e.GetDateTime(21), dateTimeFormatProperty);
        yield return new ReportCellsSourceFromDataReader("Last Visited", e => e.GetDateTime(22), dateTimeFormatProperty);
        yield return new ReportCellsSourceFromDataReader("Home Phone", e => e.GetString(23), rightAlignment);
        yield return new ReportCellsSourceFromDataReader("Work Phone", e => e.GetString(24), rightAlignment);
        yield return new ReportCellsSourceFromDataReader("Locale", e => e.GetString(25));
        yield return new ReportCellsSourceFromDataReader("Country", e => e.GetString(26), centerAlignment);
        yield return new ReportCellsSourceFromDataReader("City", e => e.GetString(27));
        yield return new ReportCellsSourceFromDataReader("Zip", e => e.GetString(28), rightAlignment);
        yield return new ReportCellsSourceFromDataReader("Address", e => e.GetString(29));
        yield return new ReportCellsSourceFromDataReader("Second Address Line", e => e.GetString(30));
        yield return new ReportCellsSourceFromDataReader("Manufacturer", e => e.GetString(31));
        yield return new ReportCellsSourceFromDataReader("Model", e => e.GetString(32));
        yield return new ReportCellsSourceFromDataReader("Vin", e => e.GetString(33), highlighted);
        yield return new ReportCellsSourceFromDataReader("Fuel Type", e => e.GetString(34));
        yield return new ReportCellsSourceFromDataReader("Type", e => e.GetString(35));
    }

    public IEnumerable<ReportCellsSourceProperty> GetGlobalProperties()
    {
        yield return new SameColumnFormatProperty();
    }
}
