# Value Providers

In general, value providers are classes that provide data for report cells. There is an interface for value providers that supply values not dependent on report data: IValueProvider. Example of such provider is SequentialNumberValueProvider - it generates sequential numbers.

## Usage Example

```c#
class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
}

VerticalReportSchemaBuilder<Customer> builder = new VerticalReportSchemaBuilder<Customer>();

// AddColumn method can accept instance of IValueProvider.
builder.AddColumn("#", new SequentialNumberValueProvider());
builder.AddColumn("Name", (Customer c) => c.Name);
builder.AddColumn("Email", (Customer c) => c.Email);

VerticalReportSchema<Customer> schema = builder.BuildSchema();

Customer[] customers = new Customer[]
{
    new Customer() {Name = "John Doe", Email = "john@example.com" },
    new Customer() {Name = "Jane Doe", Email = "jane@example.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

// Print.
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 3 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
#                    | Name                 | Email               
------------------------------------------------------------------
1                    | John Doe             | john@example.com    
2                    | Jane Doe             | jane@example.com    
*/
```

If you want to start numbering not from 1:

```c#
…
builder.AddColumn("#", new SequentialNumberValueProvider(100));
…

/*
#                    | Name                 | Email               
------------------------------------------------------------------
100                  | John Doe             | john@example.com    
101                  | Jane Doe             | jane@example.com    
*/
```

## Creating Value Provider

```c#
class NowValueProvider : IValueProvider<DateTime>
{
    public DateTime GetValue()
    {
        return DateTime.Now;
    }
}

VerticalReportSchemaBuilder<Customer> builder = new VerticalReportSchemaBuilder<Customer>();

// Use our value provider
builder.AddColumn("Generated At", new NowValueProvider());
builder.AddColumn("Name", (Customer c) => c.Name);
builder.AddColumn("Email", (Customer c) => c.Email);

VerticalReportSchema<Customer> schema = builder.BuildSchema();

Customer[] customers = new Customer[]
{
    new Customer() {Name = "John Doe", Email = "john@example.com" },
    new Customer() {Name = "Jane Doe", Email = "jane@example.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

// Print.
Console.WriteLine(string.Join("\n", reportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 3 * 23 - 3));
Console.WriteLine(string.Join("\n", reportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Generated At         | Name                 | Email               
------------------------------------------------------------------
1/6/2021 11:18:21 AM | John Doe             | john@example.com    
1/6/2021 11:18:21 AM | Jane Doe             | jane@example.com    
*/
```
