# Attribute-Based Builder

Attribute-based builder allows building report schema using attributes on your model class and class properties instead of writing code to add columns and properties.

Example:

```c#
[Alignment(Alignment.Center)]
class User
{
    [ReportColumn(1, "Name")]
    [Alignment(Alignment.Left)]
    public string Name { get; set; }

    [ReportColumn(2, "Email")]
    [Bold]
    public string Email { get; set; }

    [ReportColumn(3, "Score")]
    [DecimalPrecision(2)]
    public decimal Score { get; set; }
}

IReportSchema<User> schema = attributeBasedBuilder.BuildSchema<User>();
```

## Property Attributes

### ReportColumnAttribute

Let's imagine that we have users report with columns: name, email, age. Following class reflects the report record.

```c#
class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}
```

For report to have column for class property, ReportColumnAttribute should be added to it.

```c#
class User
{
    [ReportColumn(1, "User Name")]
    public string Name { get; set; }

    [ReportColumn(2, "Email")]
    public string Email { get; set; }

    [ReportColumn(3, "Age")]
    public int Age { get; set; }
}
```

The attribute has 2 required arguments - column order (any number, columns will be added in ascending order of this number) and column title. Building report from this model will have 3 columns: User Name, Email and Age.

Columns/rows are added with title from ReportColumn attribute. So in previous example you can work with the first column in [post-builder](#post-builder) class as:

```c#
// You can use column title
builder.ForColumn("User Name");
// or column 0-based index
builder.ForColumn(0);
// or property name as column ID
builder.ForColumn(new ColumnId(nameof(User.Name)));
```

### Complex Header

[Working example](samples/attribute-based-builder/XReports.DocsSamples.AttributeBasedBuilder.ComplexHeader/Program.cs)

Complex header can be specified using ComplexHeaderAttribute.

```c#
// Add complex header using property names.
// The first parameter is the header row number. The lower the number, the
// higher the header row is.
[ComplexHeader(1, "User Info", nameof(Name), nameof(Email), true)]
// Add complex header using columns/rows titles.
[ComplexHeader(2, "Personal Info", "Full Name", "Age")]
// Add complex header using column/row order.
// Note that numbers here are column order values - the numbers
// used in ReportColumnAttribute, not columns/rows indexes.
[ComplexHeader(2, "Contact Info", 3, 4)]
class User
{
    [ReportColumn(1, "Full Name")]
    public string Name { get; set; }

    [ReportColumn(2, "Age")]
    public int Age { get; set; }

    [ReportColumn(3, "Phone")]
    public string Phone { get; set; }

    [ReportColumn(4, "Email")]
    public string Email { get; set; }
}

/*
|                                                                                 User Info |
|----------------------|----------------------|----------------------|----------------------|
|                               Personal Info |                                Contact Info |
|----------------------|----------------------|----------------------|----------------------|
|            Full Name |                  Age |                Phone |                Email |
|----------------------|----------------------|----------------------|----------------------|
|             John Doe |                   21 |           1234567890 |  johndoe@example.com |
|             Jane Doe |                   20 |           1234567891 |  janedoe@example.com |
*/
```

### Built-in Attributes

To add properties to report cells you can add attributes above class property.

```c#
class User
{
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    [ReportColumn(2, "Email")]
    [Alignment(Alignment.Center, IsHeader = true)]
    public string Email { get; set; }

    [ReportColumn(3, "Age")]
    [Alignment(Alignment.Center)]
    public int Age { get; set; }
}
```

In this example, Age column cells will be center aligned. Also Email header cell will be center aligned.

Built-in attributes:
- **Alignment**
  - alignment (required argument): center, left or right.
- **Bold**. No arguments.
- **Color**
  - font color (required argument): color of text
  - background color (optional argument): color of background

  _you can use color name, number (e.g., 0xFF0000) or known color name_
- **CustomProperty**
  - property type (required argument): property type to assign; the property should have parameterless constructor
- **DateTimeFormat**
  - format string (required argument): c# DateTime format string
- **ExcelDateTimeFormat**
  - format string (required argument): c# DateTime format string
  - Excel format string (required argument): DateTime format string to apply when exporting to Excel
- **DecimalPrecision**
  - decimal places (required argument): decimal places to display
  - preserve trailing zeros (optional parameter, default true): flag indicating whether trailing zeros should be displayed
- **MaxLength**
  - maximum characters count (required argument): maximum length of resulting string
  - text (optional parameter, default "…"): text to append if string length is greater than maximum limit
- **PercentFormat**
  - decimal places count (required argument): decimal places to display
  - preserve trailing zeros (optional parameter, default true): flag indicating whether trailing zeros should be displayed
  - postfix text (optional parameter, default "%"): text appended after the value
- **SameColumnFormat**. Makes [EpplusWriter](./epplus-writer.md) format whole column the same as first cell in the column. Improves performance of exporting to Excel.

The attributes assign corresponding properties to row or column. To get more information about built-in properties, refer to [Properties](properties.md).

### Custom Attributes

[Working example](samples/attribute-based-builder/XReports.DocsSamples.AttributeBasedBuilder.CustomAttributes/Program.cs)

Most likely you'll have your own custom properties. You can assign them in post-builder class. But it's also possible to create attribute that will assign the property.

```c#
// Property that will be assigned to cells when MyAttribute is used.
// Assigning will be done in attribute handler class.
internal class MyProperty : ReportCellProperty
{
}

// BasePropertyAttribute has IsHeader property allowing assigning property
// to header cells.
// Also it specifies attribute usage.
internal class MyAttribute : BasePropertyAttribute
{
}

// You can implement IAttributeHandler or inherit AttributeHandler class that
// is designed to handle one type of attribute.
class MyAttributeHandler : AttributeHandler<MyAttribute>
{
    // Will be called after column or row is added.
    protected override void HandleAttribute<TSourceItem>(
        IReportSchemaBuilder<TSourceItem> schemaBuilder,
        IReportColumnBuilder<TSourceItem> columnBuilder,
        MyAttribute attribute)
    {
        if (attribute.IsHeader)
        {
            columnBuilder.AddHeaderProperties(new MyProperty());
        }
        else
        {
            columnBuilder.AddProperties(new MyProperty());
        }
    }
}
```

Last step is to use MyAttributeHandler in attribute-based builder. If you create AttributeBasedBuilder manually, you need to pass instance of MyAttributeHandler to its constructor. If you use DI to register AttributeBasedBuilder, use configuration callback when registering:

```c#
services.AddAttributeBasedBuilder(
    o => o.Add(typeof(MyAttributeHandler)));
```

Now report will have MyProperty assigned to cells in column that has MyAttribute applied.

## Horizontal Report

To build horizontal report you need to add `HorizontalReportAttribute` to class. Optionally you can mark some properties with `HeaderRowAttribute` to make them appear in table header and not in body.

```c#
[HorizontalReport]
class User
{
    [HeaderRow]
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    [ReportColumn(1, "Age")]
    public int Age { get; set; }

    [ReportColumn(2, "Phone")]
    public string Phone { get; set; }

    [ReportColumn(3, "Email")]
    public string Email { get; set; }
}
```

In this example report will have one header row and 3 rows. Exported to Html report might look like following:

```html
<table>
<thead>
    <tr>
        <th>Name</th>
        <th>John Doe</th>
    </tr>
</thead>
<tbody>
    <tr>
        <td>Age</td>
        <td>21</td>
    </tr>
    <tr>
        <td>Phone</td>
        <td>1234567890</td>
    </tr>
    <tr>
        <td>Email</td>
        <td>johndoe@example.com</td>
    </tr>
</tbody>
</table>
```

[Global attributes](#global-attributes) are **not** applied to header rows, to apply attributes you need to specify them explicitly on property:

```c#
[HorizontalReport]
[Alignment(Alignment.Center)]
class User
{
    [HeaderRow]
    [ReportColumn(1, "Name")]
    [Alignment(Alignment.Center, IsHeader = true)]
    [Alignment(Alignment.Center)]
    public string Name { get; set; }

    [ReportColumn(1, "Age")]
    public int Age { get; set; }

    [ReportColumn(2, "Phone")]
    public string Phone { get; set; }

    [ReportColumn(3, "Email")]
    public string Email { get; set; }
}
```

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

[Working example](samples/attribute-based-builder/XReports.DocsSamples.AttributeBasedBuilder.PostBuilder/Program.cs)

Sometimes using property attributes is not enough. For example, you cannot add dynamic properties using attributes. To add extra logic when building report you can provide post-builder class.

```c#
[HorizontalReport(PostBuilder = typeof(PostBuilder))]
class ReportModel
{
    [HeaderRow]
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    [ReportColumn(1, "Email")]
    public string Email { get; set; }

    [ReportColumn(2, "Score")]
    [DecimalPrecision(2)]
    public decimal Score { get; set; }

    // Post-builder class does NOT have to be nested class.
    private class PostBuilder : IReportSchemaPostBuilder<ReportModel>
    {
        // This method will be called after all columns/rows are added to schema builder.
        public void Build(IReportSchemaBuilder<ReportModel> builder, BuildOptions options)
        {
            BoldProperty boldProperty = new BoldProperty();
            builder.ForColumn(new ColumnId(nameof(Score)))
                .AddDynamicProperties(m => m.Score > 9 ? boldProperty : null);
        }
    }
}
```

**Important note:** in post-builder class to refer to columns/rows by index you need to use sequential 0-based index, and not numbers from ReportColumnAttribute. For example, in the example above to refer to "Email" column you need to use `builder.ForColumn(0)`.

Post-builder accepts instance of BuildOptions object which allows you to change type of report and count of header rows in horizontal report.

### Post-Builder Constructor Dependencies

[Working example](samples/attribute-based-builder/XReports.DocsSamples.AttributeBasedBuilder.PostBuilderConstructorDependencies/Program.cs)

Sometimes you will need to get external dependencies to be provided to post-builder class. In this case you need to register dependency in DI container and accept dependencies in constructor of post-builder class.

```c#
// Service that our post-builder class depends on.internal
class LotteryService
{
    public string GetWinner()
    {
        return new Random().Next(1, 10) % 2 == 0 ? "John" : "Jane";
    }
}

[VerticalReport(PostBuilder = typeof(PostBuilder))]
internal class UserModel
{
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    private class PostBuilder : IReportSchemaPostBuilder<UserModel>
    {
        private readonly LotteryService lotteryService;

        // Inject dependency in constructor.
        public PostBuilder(LotteryService lotteryService)
        {
            this.lotteryService = lotteryService;
        }

        public void Build(IReportSchemaBuilder<UserModel> builder, BuildOptions options)
        {
            // Use injected service.
            string winner = this.lotteryService.GetWinner();
            BoldProperty boldProperty = new BoldProperty();
            builder.ForColumn(nameof(Name))
                .AddDynamicProperties((UserModel m) => m.Name == winner ? boldProperty : null);
        }
    }
}

// Register service class in DI container so it can be injected.
services.AddScoped<LotteryService>();

// Depending on random number John or Jane will get BoldProperty assigned.
builder.BuildSchema<UserModel>().BuildReportTable(data);
```

### Parameterized Post-Builder

[Working example](samples/attribute-based-builder/XReports.DocsSamples.AttributeBasedBuilder.ParameterizedPostBuilder/Program.cs)

Sometimes it might make sense to generate slightly different report based on some input information that is known during runtime. For example, you might want to display additional column to admin users, or some properties should be applied based on request.

To solve this post-builder class may accept parameter to `Build` method.

Let's imagine that user can enter score and we want to highlight scores that are higher than it.

```c#
[VerticalReport(PostBuilder = typeof(PostBuilder))]
class UserScoreModel
{
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    [ReportColumn(2, "Score")]
    public decimal Score { get; set; }

    // Need to implement IReportSchemaPostBuilder<TModel, TParameter> interface
    // where TModel is your model type and TParameter is type of parameter
    // that will be passed during building of report schema.
    private class PostBuilder : IReportSchemaPostBuilder<UserScoreModel, decimal>
    {
        public void Build(
            IReportSchemaBuilder<UserScoreModel> builder,
            decimal minScore,
            BuildOptions options)
        {
            BoldProperty boldProperty = new BoldProperty();
            builder.ForColumn("Score")
                .AddDynamicProperties((UserScoreModel m) => m.Score > minScore ? boldProperty : null);
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

// Building report without providing build parameter will throw an exception
// as most likely this indicates an issue. If you want to be able to build
// report with or without parameter, make post-builder class implement both
// IReportSchemaPostBuilder<TModel,TParameter> and IReportSchemaPostBuilder<TModel>
// interfaces.
// The following line will throw exception in our example.
//builder.BuildSchema<UserScoreModel>().BuildReportTable(data);

// Both rows will have BoldProperty assigned to Score cells as both scores
// are greater than 80.
builder.BuildSchema<UserScoreModel, decimal>(80).BuildReportTable(data);

// Only Jane's score will have BoldProperty.
builder.BuildSchema<UserScoreModel, decimal>(90).BuildReportTable(data);
```

### Global Attributes

Often you may want all columns/rows in report to have the same attribute, for example, center alignment. To do so you may add the attribute to class.

```c#
[Alignment(Alignment.Center)]
class ReportModel
{
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    [ReportColumn(2, "Email")]
    public string Email { get; set; }

    [ReportColumn(3, "Score")]
    [DecimalPrecision(2)]
    public decimal Score { get; set; }
}

// the same as

class ReportModel
{
    [ReportColumn(1, "Name")]
    [Alignment(Alignment.Center)]
    public string Name { get; set; }

    [ReportColumn(2, "Email")]
    [Alignment(Alignment.Center)]
    public string Email { get; set; }

    [ReportColumn(3, "Score")]
    [DecimalPrecision(2)]
    [Alignment(Alignment.Center)]
    public decimal Score { get; set; }
}
```

**Important note:** any columns added in post-builder **will not** have the attribute applied.

## Table Properties

[Working example](samples/attribute-based-builder/XReports.DocsSamples.AttributeBasedBuilder.TableProperties/Program.cs)

You may add table properties using attributes.

```c#
class TitleProperty : ReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

// Inherit TableAttribute class.
class TitlePropertyAttribute : TableAttribute
{
    public TitlePropertyAttribute(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

// Need to implement IAttributeHandler.
class TitlePropertyAttributeHandler : AttributeHandler<TitlePropertyAttribute>
{
    protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, TitlePropertyAttribute attribute)
    {
        builder.AddTableProperties(new TitleProperty(attribute.Title));
    }
}

[TitleProperty("User Report")]
class User
{
    [ReportColumn(1, "First Name")]
    public string FirstName { get; set; }
    
    [ReportColumn(2, "Last Name")]
    public string LastName { get; set; }
}

// Use the attribute handler either by providing its instance to
// AttributeBasedBuilder class constructor or via configuration callback
// when registering AttributeBasedBuilder in DI, depending on you scenario.
```

Report built by this model will contain TitleProperty with Title equal to "User Report". You can use it in your writer class.

## .NET Core Integration

You need to call extension method AddAttributeBasedBuilder. This will register IAttributeBasedBuilder as singleton by default. Attribute handlers and post-builders can be registered in DI container, but they don't have to, even if they have dependencies that need to be resolved from service provider.

```c#
// You can pass configuration callback and service lifetime.
// Example below registers attribute-based builder in DI container with
// singleton lifetime, this builder will use all attribute handlers
// (implementors of IAttributeHandler interface) from
// executing assembly.
services.AddAttributeBasedBuilder(
    o => o.AddFromAssembly(Assembly.GetExecutingAssembly()),
    ServiceLifetime.Singleton);
```
