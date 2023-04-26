# Value Providers

Value providers are classes that provide values that do not require report data. There is an interface for value providers that supply such values: IValueProvider. Example of such provider is SequentialNumberValueProvider - it generates sequential numbers.

## Usage Example

[Working example](../../docs-samples/value-providers/XReports.DocsSamples.ValueProviders.UsageExample)

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
}

ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();

// AddColumn can accept instance of IValueProvider.
builder.AddColumn("#", new SequentialNumberValueProvider());
builder.AddColumn("Name", (Customer c) => c.Name);
builder.AddColumn("Email", (Customer c) => c.Email);

IReportSchema<Customer> schema = builder.BuildVerticalSchema();

Customer[] customers = new Customer[]
{
    new Customer() { Name = "John Doe", Email = "john@example.com" },
    new Customer() { Name = "Jane Doe", Email = "jane@example.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

new SimpleConsoleWriter().Write(reportTable);

/*
|                    # |                 Name |                Email |
----------------------------------------------------------------------
|                    1 |             John Doe |     john@example.com |
|                    2 |             Jane Doe |     jane@example.com |
*/
```

If you want to start numbering not from 1:

```c#
…
builder.AddColumn("#", new SequentialNumberValueProvider(100));
…

/*
|                    # |                 Name |                Email |
----------------------------------------------------------------------
|                  100 |             John Doe |     john@example.com |
|                  101 |             Jane Doe |     jane@example.com |
*/
```

## Creating Value Provider

[Working example](../../docs-samples/value-providers/XReports.DocsSamples.ValueProviders.CreatingValueProvider)

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
}

class DayOfWeekProvider : IValueProvider<string>
{
    private readonly Lazy<DateTime> today = new(() => DateTime.Today);

    public string GetValue()
    {
        return this.today.Value.ToString("dddd", CultureInfo.InvariantCulture);
    }
}

ReportSchemaBuilder<Customer> builder = new ReportSchemaBuilder<Customer>();

// Use our value provider
builder.AddColumn("Generated On", new DayOfWeekProvider());
builder.AddColumn("Name", (Customer c) => c.Name);
builder.AddColumn("Email", (Customer c) => c.Email);

IReportSchema<Customer> schema = builder.BuildVerticalSchema();

Customer[] customers = new Customer[]
{
    new Customer() { Name = "John Doe", Email = "john@example.com" },
    new Customer() { Name = "Jane Doe", Email = "jane@example.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

new SimpleConsoleWriter().Write(reportTable);

/*
|         Generated On |                 Name |                Email |
----------------------------------------------------------------------
|            Wednesday |             John Doe |     john@example.com |
|            Wednesday |             Jane Doe |     jane@example.com |
*/
```
