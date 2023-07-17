using XReports.Converter;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<UserInfo> builder = new ReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", (UserInfo u) => u.Username);
builder.AddColumn("Email", (UserInfo u) => u.Email)
    .AddProperties(new MyMaxLengthProperty(true, 15));
builder.AddColumn("Biography", (UserInfo u) => u.Bio)
    .AddProperties(new MaxLengthProperty(10));

builder.AddComplexHeader(0, "User Info", 0, 1);

IReportSchema<UserInfo> schema = builder.BuildVerticalSchema();

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Email = "guest@example.com", Bio = "Lorem ipsum" },
    new UserInfo() { Username = "admin", Email = "admin@gmail.com", Bio = "Lorem ipsum" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);
HtmlStringWriter writer = new HtmlStringWriter(new HtmlStringCellWriter());

IReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
{
    new AllMaxLengthPropertiesHtmlHandler(),
});

Console.WriteLine($"Using {nameof(AllMaxLengthPropertiesHtmlHandler)}");
Console.WriteLine(writer.Write(converter.Convert(reportTable)));
Console.WriteLine();

converter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
{
    new MaxLengthPropertyHtmlHandler(),
    new MyMaxLengthPropertyHtmlHandler(),
});

Console.WriteLine($"Using {nameof(MaxLengthPropertyHtmlHandler)} and {nameof(MyMaxLengthPropertyHtmlHandler)}");
Console.WriteLine(writer.Write(converter.Convert(reportTable)));

internal class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
}

internal class MyMaxLengthProperty : MaxLengthProperty
{
    public MyMaxLengthProperty(bool showFullText, int maxLength)
        : base(maxLength)
    {
        this.ShowFullText = showFullText;
    }

    public bool ShowFullText { get; }
}

internal class AllMaxLengthPropertiesHtmlHandler : MaxLengthPropertyHtmlHandler
{
    public override int Priority => base.Priority - 1;

    protected override void HandleProperty(MaxLengthProperty property, HtmlReportCell cell)
    {
        switch (property)
        {
            case MyMaxLengthProperty myMaxLengthProperty:
                this.HandleProperty(myMaxLengthProperty, cell);
                break;
            default:
                base.HandleProperty(property, cell);
                break;
        }
    }

    private void HandleProperty(MyMaxLengthProperty property, HtmlReportCell cell)
    {
        // If user does not want to see full text, we fallback to default
        // inherited behavior.
        if (!property.ShowFullText)
        {
            base.HandleProperty(property, cell);

            return;
        }

        // In order not to duplicate logic of determining if text is too long,
        // we save text before handler and after. If it's different, it's been
        // truncated.
        string fullText = cell.GetValue<string>();

        base.HandleProperty(property, cell);

        if (fullText != cell.GetValue<string>())
        {
            cell.Attributes["title"] = fullText;
        }
    }
}

internal class MyMaxLengthPropertyHtmlHandler : PropertyHandler<MyMaxLengthProperty, HtmlReportCell>
{
    private readonly MaxLengthPropertyHtmlHandler originalHandler = new MaxLengthPropertyHtmlHandler();

    public override int Priority => this.originalHandler.Priority - 1;

    protected override void HandleProperty(MyMaxLengthProperty property, HtmlReportCell cell)
    {
        // If user does not want to see full text, we fallback to default behavior.
        if (!property.ShowFullText)
        {
            this.originalHandler.Handle(property, cell);

            return;
        }

        // In order not to duplicate logic of determining if text is too long,
        // we save text before handler and after. If it's different, it's been
        // truncated.
        string fullText = cell.GetValue<string>();

        this.originalHandler.Handle(property, cell);

        if (fullText != cell.GetValue<string>())
        {
            cell.Attributes["title"] = fullText;
        }
    }
}
