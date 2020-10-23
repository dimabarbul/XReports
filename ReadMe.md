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
    public async Task<IActionResult> GetSurveyListAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = await this.reportService.BuildReportAsync(filter);

        DataTablesReportWriter reportWriter = new DataTablesReportWriter(new BootstrapHtmlReportConverter().Convert(reportTable));

        return Json(new
        {
            Filters = this.GetFilters(),
            Data = reportWriter.Write(),
        });
    }

    public async Task<IActionResult> DisplaySurveyListAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = await this.reportService.BuildReportAsync(filter);

        StringReportWriter reportWriter = new StringReportWriter(new BootstrapHtmlReportConverter().Convert(reportTable));

        return View(new
        {
            ReportHtml = reportWriter.Write(),
        });
    }

    public async Task<IActionResult> ExportToExcelAsync(SurveyListFilter filter)
    {
        ReportTable reportTable = this.reportService.BuildReportAsync(filter);

        ExcelReportWriter reportWriter = new ExcelReportWriter(new StandardExcelReportConverter().Convert(reportTable));

        return new FileStreamResult(reportWriter.WriteToStream(), MimeType.Application.Xlsx)
        {
            FileDownloadName = "SurveyList.xlsx",
        };
    }
}
```
