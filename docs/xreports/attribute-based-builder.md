# AttributeBasedBuilder

AttributeBasedBuilder allows building report schema using attributes on class properties.

## .NET Core Integration

You need to call extension method AddAttributeBasedBuilder. This will register IAttributeBasedBuilder. Attribute handlers and post-builders can be registered in DI container.

```c#
class ReportModel
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    [ReportVariable(2, "Email")]
    public string Email { get; set; }

    [ReportVariable(3, "Score")]
    [DecimalPrecision(2)]
    public decimal Score { get; set; }
}

ServiceCollection services = new ServiceCollection();
// Register IAttributeBasedBuilder
services.AddAttributeBasedBuilder();
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
- **DecimalPrecision**. Has 1 required argument - decimal places count.
- **MaxLength**. Has 1 required argument - maximum characters count.
- **PercentFormat**. Has 1 required argument - decimal places count. Optionally may have PostfixText - text appended after percent value.
- **SameColumnFormat**. No arguments. Makes EpplusWriter format whole column the same as first cell in the column. Improves performance.

The attributes assign corresponding properties to row or column. To get more information about built-in properties, refer to [Properties](properties.md);

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
    [DecimalPrecision(2)]
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

### Constructor Dependencies

Sometimes you will need to get external dependencies to be provided to post-builder class. IN this case you need to register dependency in DI container and accept dependencies in constructor of post-builder class.

```c#
// Service our post-builder class depends on.
class LotteryService
{
    public string GetWinner()
    {
        return new Random().Next(1, 10) % 2 == 0 ? "John" : "Jane";
    }
}

[VerticalReport(PostBuilder = typeof(PostBuilder))]
class UserModel
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    private class PostBuilder : IVerticalReportPostBuilder<UserModel>
    {
        private readonly LotteryService lotteryService;

        // Inject dependency in constructor.
        public PostBuilder(LotteryService lotteryService)
        {
            this.lotteryService = lotteryService;
        }

        public void Build(VerticalReportSchemaBuilder<UserModel> builder)
        {
            // Use injected service.
            string winner = this.lotteryService.GetWinner();
            builder.ForColumn(nameof(Name))
                .AddDynamicProperty((UserModel m) => m.Name == winner ? new BoldProperty() : null);
        }
    }
}

ServiceCollection services = new ServiceCollection();
services.AddAttributeBasedBuilder()
    // Register service class in DI container so it can be injected.
    .AddScoped<LotteryService>();
ServiceProvider serviceProvider = services.BuildServiceProvider();

IAttributeBasedBuilder builder = serviceProvider.GetRequiredService<IAttributeBasedBuilder>();

UserModel[] data =
{
    new UserModel() { Name = "John" },
    new UserModel() { Name = "Jane" },
};

// Depending on random number John or Jane will get BoldProperty assigned.
builder.BuildSchema<UserModel>().BuildReportTable(data);
```

### Parameterized Post-Builder

Sometimes it might make sense to generate slightly different report based on some input information that is known during runtime. For example, You might want to display additional column to admin users, or some properties should be applied based on request.

To solve this post-builder class may accept parameter to `Build` method.

Let's imagine that user can enter score and we want to highlight scores that are higher than it.

```c#
[VerticalReport(PostBuilder = typeof(PostBuilder))]
class UserScoreModel
{
    [ReportVariable(1, "Name")]
    public string Name { get; set; }

    [ReportVariable(2, "Score")]
    public decimal Score { get; set; }

    // Need to implement IVerticalReportPostBuilder<TModel, TParameter> interface
    // where TModel is your model type and TParameter is type of parameter
    // that will be passed during building of report schema.
    private class PostBuilder : IVerticalReportPostBuilder<UserScoreModel, decimal>
    {
        public void Build(VerticalReportSchemaBuilder<UserScoreModel> builder, decimal minScore)
        {
            builder.ForColumn(nameof(Score))
                .AddDynamicProperty((UserScoreModel m) => m.Score > minScore ? new BoldProperty() : null);
        }
    }
}

ServiceCollection services = new ServiceCollection();
services.AddAttributeBasedBuilder();
ServiceProvider serviceProvider = services.BuildServiceProvider();

IAttributeBasedBuilder builder = serviceProvider.GetRequiredService<IAttributeBasedBuilder>();

UserScoreModel[] data =
{
    new UserScoreModel() { Name = "John", Score = 90 },
    new UserScoreModel() { Name = "Jane", Score = 100 },
};

// As we do not provide any arguments, post-builder Build method won't be called.
builder.BuildSchema<UserScoreModel>().BuildReportTable(data);

// Both rows will have BoldProperty assigned to Score cells as both scores
// are greater than 80.
builder.BuildSchema<UserScoreModel, decimal>(80).BuildReportTable(data);

// Only Jane's score will have BoldProperty.
builder.BuildSchema<UserScoreModel, decimal>(90).BuildReportTable(data);
```

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
    [DecimalPrecision(2)]
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
    [DecimalPrecision(2)]
    [Alignment(AlignmentType.Center)]
    public decimal Score { get; set; }
}
```

**Important note:** any columns/rows added in post-builder **will not** have the attribute applied.
