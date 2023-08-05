using System.Data;
using System.Text.Json;
using XReports.Benchmarks.Models;

namespace XReports.Benchmarks;

public static class DataProvider
{
    private const string DataPath = "data.json";

    private static readonly Person[] Data;

    static DataProvider()
    {
        using FileStream fileStream = File.OpenRead(DataPath);
        Data = JsonSerializer.Deserialize<Person[]>(fileStream);
    }

    public static Person[] GetData(int recordsCount)
    {
        Person[] result = new Person[recordsCount];
        int toCopy = recordsCount;
        int resultIndex = 0;

        while (toCopy > 0)
        {
            int toCopyOnThisStep = Math.Min(toCopy, Data.Length);
            Array.Copy(Data, 0, result, resultIndex, toCopyOnThisStep);
            resultIndex += toCopyOnThisStep;
            toCopy -= toCopyOnThisStep;
        }

        return result;
    }

    public static DataTable CreateDataTable(IReadOnlyList<Person> people)
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

        for (int i = 0; i < people.Count; i++)
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
