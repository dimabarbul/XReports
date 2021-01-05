# Cell Processors

Cell processors allow you to perform any actions on cells. In most cases it is enough to use properties and dynamic properties, but in some rare cases you may want to process cell before conversion.

Let's imagine that we want to display list of customers and their count of orders. We'd like to highlight names of customers who purchased less than 10 orders. Also we'd like to highlight name in different way when customer has more than 100 orders.

Sure, we can do this with dynamic property, but logic might be complex. So, you might want to move the logic to separate class.

## Cell Processor

Cell processor should implement IReportCellProcessor interface.

```c#
// Data model.
class CustomerInfo
{
    public string Name { get; set; }
    public int OrdersCount { get; set; }
}

// Property used to mark cells to highlight.
class HighlightProperty : ReportCellProperty
{
    // Symbol to mark highlighted cell with.
    public char Symbol { get; }

    public HighlightProperty(char symbol)
    {
        this.Symbol = symbol;
    }
}

// Implements logic to highlight cell.
class HighlightPropertyHandler : PropertyHandler<HighlightProperty, ReportCell>
{
    protected override void HandleProperty(HighlightProperty property, ReportCell cell)
    {
        string value = cell.GetValue<string>();
        cell.Value = $"{value} ({property.Symbol})";
    }
}

// Processor will be called after all properties are assigned to the cell.
class HighlightByOrderCountProcessor : IReportCellProcessor<CustomerInfo>
{
    public void Process(ReportCell cell, CustomerInfo entity)
    {
        if (entity.OrdersCount < 10)
        {
            cell.AddProperty(new HighlightProperty('-'));
        }
        else if (entity.OrdersCount > 100)
        {
            cell.AddProperty(new HighlightProperty('+'));
        }
    }
}

VerticalReportSchemaBuilder<CustomerInfo> builder = new VerticalReportSchemaBuilder<CustomerInfo>();

builder.AddColumn("Name", (CustomerInfo c) => c.Name)
    // processor will be called for each cell in the column
    .AddProcessors(new HighlightByOrderCountProcessor());
builder.AddColumn("Orders #", (CustomerInfo c) => c.OrdersCount);

VerticalReportSchema<CustomerInfo> schema = builder.BuildSchema();

CustomerInfo[] customers = new CustomerInfo[]
{
    new CustomerInfo() { Name = "John", OrdersCount = 5},
    new CustomerInfo() { Name = "Jane", OrdersCount = 50},
    new CustomerInfo() { Name = "David", OrdersCount = 120},
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(customers);

// Convert report. This will run property handlers.
IReportConverter<ReportCell> converter = new ReportConverter<ReportCell>(new []
{
    new HighlightPropertyHandler(),
});
IReportTable<ReportCell> convertedReportTable = converter.Convert(reportTable);

// Write report to console.
Console.WriteLine(string.Join("\n", convertedReportTable.HeaderRows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));
Console.WriteLine(new string('-', 2 * 23 - 3));
Console.WriteLine(string.Join("\n", convertedReportTable.Rows.Select(row =>
    string.Join(" | ", row.Select(c => string.Format("{0,-20}", c.GetValue<string>())))
)));

/*
Name                 | Orders #            
-------------------------------------------
John (-)             | 5                   
Jane                 | 50                  
David (+)            | 120                 
*/
```

##  Header Cell Processor

Just like for body cells, you can add processor for header cells. You can define processors only for regular header cells, not complex header ones.
