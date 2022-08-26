namespace XReports.Benchmarks.Core.Models;

public class Person
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
    public string LastIpAddress { get; set; }
    public string BrowserUserAgent { get; set; }
    public string Text { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime LastVisitedAt { get; set; }
    public string HomePhone { get; set; }
    public string WorkPhone { get; set; }
    public string Locale { get; set; }
    public Vehicle Vehicle { get; set; }
}
