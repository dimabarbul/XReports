using XReports.BenchmarksCore.Models;

namespace XReports.BenchmarksCore;

public class DataProvider
{
    public IEnumerable<Person> GetData(int recordsCount)
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
            .Concat(dataX5);
    }

}
