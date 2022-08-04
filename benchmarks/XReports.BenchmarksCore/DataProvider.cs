using System.Data;
using XReports.BenchmarksCore.Models;

namespace XReports.BenchmarksCore;

public static class DataProvider
{
    public static Person[] GetData(int recordsCount)
    {
        int luckyGuyIndex = new Random().Next(3, recordsCount - 1);

        List<Person> data = new Bogus.Faker<Person>()
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
            .Generate(recordsCount / 20);

        IEnumerable<Person> dataX5 = data
            .Concat(data)
            .Concat(data)
            .Concat(data)
            .Concat(data);

        return dataX5
            .Concat(dataX5)
            .Concat(dataX5)
            .Concat(dataX5)
            .ToArray();
    }

    public static DataTable CreateDataTable(Person[] people)
    {
        DataTable dataTable = new();

        dataTable.Columns.AddRange(new[]
        {
            new DataColumn("FirstName"),
            new DataColumn("LastName"),
            new DataColumn("Email"),
            new DataColumn("Score", typeof(decimal)),
            new DataColumn("Account Number"),
            new DataColumn("Btc Wallet"),
            new DataColumn("Eth Wallet"),
            new DataColumn("Account #", typeof(decimal)),
            new DataColumn("Btc #", typeof(decimal)),
            new DataColumn("Eth #", typeof(decimal)),
            new DataColumn("Bio"),
            new DataColumn("DOB", typeof(DateTime)),
            new DataColumn("Company Name"),
            new DataColumn("Preferred Color"),
            new DataColumn("Avatar"),
            new DataColumn("Password"),
            new DataColumn("Username"),
            new DataColumn("Home Page"),
            new DataColumn("Last IP"),
            new DataColumn("User Agent"),
            new DataColumn("Lorem Ipsum"),
            new DataColumn("Registered", typeof(DateTime)),
            new DataColumn("Last Visited", typeof(DateTime)),
            new DataColumn("Home Phone"),
            new DataColumn("Work Phone"),
            new DataColumn("Locale"),
            new DataColumn("Country"),
            new DataColumn("City"),
            new DataColumn("Zip"),
            new DataColumn("Address"),
            new DataColumn("Second Address Line"),
            new DataColumn("Manufacturer"),
            new DataColumn("Model"),
            new DataColumn("Vin"),
            new DataColumn("Fuel Type"),
            new DataColumn("Type"),
        });

        for (int i = 0; i < people.Length; i++)
        {
            DataRow dataRow = dataTable.NewRow();

            dataRow["FirstName"] = people[i].FirstName;
            dataRow["LastName"] = people[i].LastName;
            dataRow["Email"] = people[i].Email;
            dataRow["Score"] = people[i].Score;
            dataRow["Account Number"] = people[i].AccountNumber;
            dataRow["Btc Wallet"] = people[i].BtcAddress;
            dataRow["Eth Wallet"] = people[i].EthAddress;
            dataRow["Account #"] = people[i].AccountAmount;
            dataRow["Btc #"] = people[i].BtcAmount;
            dataRow["Eth #"] = people[i].EthAmount;
            dataRow["Bio"] = people[i].Bio;
            dataRow["DOB"] = people[i].DateOfBirth;
            dataRow["Company Name"] = people[i].CompanyName;
            dataRow["Preferred Color"] = people[i].PreferredColor;
            dataRow["Avatar"] = people[i].Avatar;
            dataRow["Password"] = people[i].Password;
            dataRow["Username"] = people[i].UserName;
            dataRow["Home Page"] = people[i].HomePage;
            dataRow["Last IP"] = people[i].LastIpAddress;
            dataRow["User Agent"] = people[i].BrowserUserAgent;
            dataRow["Lorem Ipsum"] = people[i].Text;
            dataRow["Registered"] = people[i].RegisteredAt;
            dataRow["Last Visited"] = people[i].LastVisitedAt;
            dataRow["Home Phone"] = people[i].HomePhone;
            dataRow["Work Phone"] = people[i].WorkPhone;
            dataRow["Locale"] = people[i].Locale;
            dataRow["Country"] = people[i].Address.Country;
            dataRow["City"] = people[i].Address.City;
            dataRow["Zip"] = people[i].Address.ZipCode;
            dataRow["Address"] = people[i].Address.StreetAddress1;
            dataRow["Second Address Line"] = people[i].Address.StreetAddress2;
            dataRow["Manufacturer"] = people[i].Vehicle.Manufacturer;
            dataRow["Model"] = people[i].Vehicle.Model;
            dataRow["Vin"] = people[i].Vehicle.Vin;
            dataRow["Fuel Type"] = people[i].Vehicle.FuelType;
            dataRow["Type"] = people[i].Vehicle.Type;

            dataTable.Rows.Add(dataRow);
        }

        return dataTable;
    }
}
