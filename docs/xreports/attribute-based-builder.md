# AttributeBasedBuilder

AttributeBasedBuilder allows building report schema using attributes on class properties.

## .NET Core Integration

You need to call extension method UseAttributeBasedBuilder. This will register IAttributeBasedBuilder with all available attribute handlers (more on this later) and all post-builder classes.

```c#
class ReportModel
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    [ReportVariable(2, "Email")]
    public string Email { get; set; }

    [ReportVariable(3, "Score")]
    [DecimalFormat(2)]
    public decimal Score { get; set; }
}

ServiceCollection services = new ServiceCollection();
// Register IAttributeBasedBuilder and all post-builder classes.
services.UseAttributeBasedBuilder();
ServiceProvider serviceProvider = services.BuildServiceProvider();

// Get builder.
IAttributeBasedBuilder builder = serviceProvider.GetRequiredService<IAttributeBasedBuilder>();
// Building schema is matter of calling one method passing your model type.
// Everything will be configured by attributes and optionally in post-builder class.
IReportSchema<ReportModel> schema = builder.BuildSchema<ReportModel>();
```

## Property Attributes

### ReportVariableAttribute

Let's imagine that we have users report with columns: name, email, age. Following class reflects the report record.

```c#
class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}
```

For report to have column for class property, ReportVariableAttribute should be added to it.

```c#
class User
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    [ReportVariable(2, "Email")]
    public string Email { get; set; }

    [ReportVariable(3, "Age")]
    public int Age { get; set; }
}
```

The attribute has 2 required arguments - column order (any number, columns will be added in ascending order of this number) and column title. Building report from this model will have 3 columns: Name, Email and Age.

### Complex Header

Complex header can be specified as optional argument to ReportVariableAttribute.

```c#
class User
{
    [ReportVariable(1, "Name", ComplexHeader = new[] { "Personal Info" })]
    public string Name { get; set; }

    [ReportVariable(2, "Age", ComplexHeader = new[] { "Personal Info" })]
    public int Age { get; set; }

    [ReportVariable(3, "Name", ComplexHeader = new[] { "Personal Info", "Contact Info" })]
    public string Phone { get; set; }

    [ReportVariable(4, "Email", ComplexHeader = new[] { "Personal Info", "Contact Info" })]
    public string Email { get; set; }
}
```

In this example report will have one complex header row (topmost) "Personal Info" combining all columns and second complex header row "Contact Info" combining Phone and Email columns. Exported to Html report might look like following:

```html
<table>
<thead>
    <tr>
        <th colSpan="4">Personal Info</th>
    </tr>
    <tr>
        <th rowSpan="2">Name</th>
        <th rowSpan="2">Age</th>
        <th colSpan="2">Contact Info</th>
    </tr>
    <tr>
        <th>Name</th>
        <th>Email</th>
    </tr>
</thead>
</table>
```

### Built-in Attributes

To add properties to report cells you can add attributes above needed class property.

```c#
class User
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    [ReportVariable(2, "Email")]
    [Alignment(AlignmentType.Center, IsHeader = true)]
    public string Email { get; set; }

    [ReportVariable(3, "Age")]
    [Alignment(AlignmentType.Center)]
    public int Age { get; set; }
}
```

In this example, Age column cells will be center aligned. Also Email header cell will be center aligned.

Built-in attributes:
- **Alignment**. Has 1 required argument - AlignmentType - center, left or right.
- **Bold**. No arguments.
- **Color**. Has 1 (font color) or 2 (font and background colors) required arguments.
- **CustomProperty**. Has 1 required argument - type of property to assign. Property should have no constructor arguments.
- **DateTimeFormat**. Has 1 required argument - format string - c# DateTime format string.
- **DecimalFormat**. Has 1 required argument - decimal places count.
- **MaxLength**. Has 1 required argument - maximum characters count.
- **PercentFormat**. Has 1 required argument - decimal places count. Optionally may have PostfixText - text appended after percent value.
- **SameColumnFormat**. No arguments. Makes EpplusWriter format whole column the same as first cell in the column. Improves performance.

### Custom Attributes

Most likely you'll have your own custom properties. You can assign them in post-builder class. But it's also possible to create attribute that will assign the property.

```c#
class MyProperty : ReportCellProperty
{
}

// AttributeBase has IsHeader property allowing assigning property to header cells.
// Also it specifies attribute usage.
class MyAttribute : AttributeBase
{
}

// You can implement IAttributeHandler or inherit AttributeHandler class that
// is designed to handle one type of attribute.
class MyAttributeHandler : AttributeHandler<MyAttribute>
{
    // Will be called after column or row is added.
    protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, MyAttribute attribute)
    {
        // Current builder column/row is the one having our MyAttribute.
        builder.AddProperties(new MyProperty());
    }
}

class User
{
    [ReportVariable(1, "Name")]
    [My]
    public string Name { get; set; }
}
```

MyAttributeHandler will be picked up automatically during registering IAttributeBasedBuilder in DI container. Report created from model above will have MyProperty assigned to Name column.

## Class attributes

### Horizontal/Vertical Report Attribute

Class may have one of two attributes specifying report type: HorizontalReportAttribute or VerticalReportAttribute. If class has no attribute specified, report is considered vertical.

```c#
[HorizontalReport]
class HorizontalReportModel
{
}

[VerticalReport]
class HorizontalReportModel
{
}
```

### Post-Builder

Sometimes using property attributes is not enough. For example, when you're making horizontal report, you cannot add header rows using attributes. To add extra logic when building report you can provide post-builder class.

```c#
[HorizontalReport(PostBuilder = typeof(PostBuilder))]
class ReportModel
{
    public string Name { get; set; }

    [ReportVariable(1, "Email")]
    public string Email { get; set; }

    [ReportVariable(2, "Score")]
    [DecimalFormat(2)]
    public decimal Score { get; set; }

    // Post-builder class does NOT have to be nested class.
    // It's registered in DI container, so it can have constructor with dependencies.
    private class PostBuilder : IHorizontalReportPostBuilder<ReportModel>
    {
        // This method will be called after all columns/rows are added to schema builder.
        public void Build(HorizontalReportSchemaBuilder<ReportModel> builder)
        {
            // add any custom actions here
            builder.AddHeaderRow(string.Empty, (ReportModel r) => r.Name)
                .AddProperties(new AlignmentProperty(AlignmentType.Center));
        }
    }
}
```

For vertical report post-builder class should implement IVerticalReportPostBuilder interface.

### Table Properties

Often you may want all columns/rows in report to have the same attribute, for example, center alignment. To do so you may add the attribute to class.

```c#
[Alignment(AlignmentType.Center)]
class ReportModel
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    [ReportVariable(2, "Email")]
    public string Email { get; set; }

    [ReportVariable(3, "Score")]
    [DecimalFormat(2)]
    public decimal Score { get; set; }
}

// the same as

class ReportModel
{
    [ReportVariable(1, "Name")]
    [Alignment(AlignmentType.Center)]
    public string Name { get; set; }

    [ReportVariable(2, "Email")]
    [Alignment(AlignmentType.Center)]
    public string Email { get; set; }

    [ReportVariable(3, "Score")]
    [DecimalFormat(2)]
    [Alignment(AlignmentType.Center)]
    public decimal Score { get; set; }
}
```

**Important note:** any columns/rows added in post-builder **will not** have the attribute applied.