# About
This project is intended to provide basic but extendable way of building reports and exporting them to different formats: HTML, Excel etc.

# Example of Code Using the Library
Example of using the library:
```c#
public class MyReportColumnAttribute : ReportColumnAttribute
{
    public bool Translatable { get; set; }
    public bool Sortable { get; set; }
}

public class SurveyListItemDto
{
    [MyReportColumn(1, "Survey ID")]
    public int SurveyId { get; set; }

    [MyReportColumn(2, "Survey Start Date", Format = Constants.DateFormats.DefaultDateFormat)]
    public DateTime SurveyDate { get; set; }

    [MyReportColumn(3, "Type", Translatable = true)]
    public string SurveyType { get; set; }

    [MyReportColumn(4, "Survey Answers", "Q1. OSAT", Sortable = false)]
    public int Q1 { get; set; }

    [MyReportColumn(5, "Survey Answers", "Q2. Name", Sortable = false)]
    public string Q2 { get; set; }

    [MyReportColumn(6, "Survey Answers", "Score", Format = "F2")]
    public decimal SurveyScore { get; set; }
}

public class SurveyListReportBuilder : EntityVerticalReportBuilder<SurveyListItemDto>
{
    public ReportTable Build()
    {
        ReportTable table = base.Build();

        table.InsertColumn(0, "#", new ValueProviderReportColumn<SurveyListItemDto, int>(new SequentialNumberValueProvider()));

        table.AddColumnProperty("Survey ID", new LinkToPopupProperty<SurveyListItemDto>(
            (SurveyListItemDto entity) => UrlBuilder.ToAction("SurveyDetails", "Survey", new { SurveyId = entity.SurveyId, Type = entity.SurveyType })
        ));

        table.AddColumnProperty("Survey Start Date", new HoverProperty<SurveyListItemDto>(
            (SurveyListItemDto entity) => $"Completed {(DateTime.Now - entity.SurveyDate).Days} days ago"
        ));

        return table;
    }
}

public class ReportController : Controller
{
    private readonly Func<string, IEnumerable<IHtmlPropertyHandler>> htmlPropertyHandlersFactory;
    private readonly Func<string, IEnumerable<IExcelPropertyHandler>> excelPropertyHandlersFactory;

    public ReportController(Func<string, IEnumerable<IHtmlPropertyHandler>> htmlPropertyHandlersFactory, Func<string, IEnumerable<IExcelPropertyHandler>> excelPropertyHandlersFactory)
    {
        this.htmlPropertyHandlersFactory = htmlPropertyHandlersFactory;
        this.excelPropertyHandlersFactory = excelPropertyHandlersFactory;
    }

    public async Task<IActionResult> GetSurveyListAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = await this.reportService.BuildReportAsync(filter);

        HtmlReportConverter reportConverter = new HtmlReportConverter(this.htmlPropertyHandlersFactory(HtmlPropertyHandlerType.Bootstrap));
        HtmlReportTable htmlReportTable = reportConverter.Convert(reportTable);
        DataTablesReportWriter reportWriter = new DataTablesReportWriter(htmlReportTable);

        return Json(new
        {
            Filters = this.GetFilters(),
            Data = reportWriter.Write(),
        });
    }

    public async Task<IActionResult> DisplaySurveyListAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = await this.reportService.BuildReportAsync(filter);

        HtmlReportConverter reportConverter = new HtmlReportConverter(this.htmlPropertyHandlersFactory(HtmlPropertyHandlerType.Bootstrap));
        HtmlReportTable htmlReportTable = reportConverter.Convert(reportTable);
        StringReportWriter reportWriter = new StringReportWriter(htmlReportTable);

        return View(new
        {
            ReportHtml = reportWriter.Write(),
        });
    }

    public async Task<IActionResult> ExportToExcelAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = await this.reportService.BuildReportAsync(filter);

        ExcelReportConverter reportConverter = new ExcelReportConverter(this.excelPropertyHandlersFactory(ExcelPropertyHandlerType.Standard));
        ExcelReportTable excelReportTable = reportConverter.Convert(reportTable);
        ExcelReportWriter reportWriter = new ExcelReportWriter(excelReportTable);

        return new FileStreamResult(reportWriter.WriteToStream(), MimeType.Application.Xlsx)
        {
            FileDownloadName = "SurveyList.xlsx",
        };
    }

    public async Task<IActionResult> SendToEmailAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = await this.reportService.BuildReportAsync(filter);
        string email = this.GetCurrentUserEmail();

        HtmlReportConverter reportConverter = new HtmlReportConverter(this.htmlPropertyHandlersFactory(HtmlPropertyHandlerType.Standard));
        HtmlReportTable htmlReportTable = reportConverter.Convert(reportTable);
        StringReportWriter reportWriter = new StringReportWriter(htmlReportTable);

        await this.emailService.SendHtmlAsync(email, subject: "Survey List Report", body: reportWriter.Write());
    }
}
```
