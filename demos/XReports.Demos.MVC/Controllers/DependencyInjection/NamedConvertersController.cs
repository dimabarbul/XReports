using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Demos.MVC.Models.Shared;
using XReports.DependencyInjection;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.Writers;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.Demos.MVC.Controllers.DependencyInjection;

public class NamedConvertersController : Controller
{
    private const int RecordsCount = 10;

    private readonly IAttributeBasedBuilder attributeBasedBuilder;
    private readonly IReportConverterFactory<HtmlReportCell> htmlConverterFactory;
    private readonly IReportConverterFactory<ExcelReportCell> excelConverterFactory;
    private readonly IHtmlStringWriter htmlWriter;
    private readonly IEpplusWriter excelWriter;

    public NamedConvertersController(
        IAttributeBasedBuilder attributeBasedBuilder,
        IReportConverterFactory<HtmlReportCell> htmlConverterFactory,
        IReportConverterFactory<ExcelReportCell> excelConverterFactory,
        IHtmlStringWriter htmlWriter,
        IEpplusWriter excelWriter)
    {
        this.attributeBasedBuilder = attributeBasedBuilder;
        this.htmlConverterFactory = htmlConverterFactory;
        this.excelConverterFactory = excelConverterFactory;
        this.htmlWriter = htmlWriter;
        this.excelWriter = excelWriter;
    }

    public IActionResult Index()
    {
        string tableHtml = this.GetReportHtml();

        return this.View(new ReportViewModel()
        {
            ReportTableHtml = tableHtml,
        });
    }

    public IActionResult Download()
    {
        Stream excelStream = this.GetExcelStream();

        return this.File(excelStream, Constants.ContentTypeExcel, "NamedConverters.xlsx");
    }

    private string GetReportHtml()
    {
        IReportTable<ReportCell> reportTable = this.attributeBasedBuilder.BuildSchema<Entity>()
            .BuildReportTable(this.GetData());
        IReportTable<HtmlReportCell> htmlReportTable = this.htmlConverterFactory.Get("no-handlers")
            .Convert(reportTable);
        string html = this.htmlWriter.WriteToString(htmlReportTable);

        return html;
    }

    private Stream GetExcelStream()
    {
        IReportTable<ReportCell> reportTable = this.attributeBasedBuilder.BuildSchema<Entity>()
            .BuildReportTable(this.GetData());
        IReportTable<ExcelReportCell> excelReportTable = this.excelConverterFactory.Get("no-handlers")
            .Convert(reportTable);
        Stream excelStream = this.excelWriter.WriteToStream(excelReportTable);

        return excelStream;
    }

    private IEnumerable<Entity> GetData()
    {
        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Score, f => f.Random.Decimal(0, 100))
            .Generate(RecordsCount);
    }

    private class Entity
    {
        [ReportColumn(1, "Full name")]
        public string Name { get; set; }

        [ReportColumn(2, "Score")]
        // DecimalPrecisionAttribute assigns DecimalPrecisionProperty which will be ignored
        // because the "no-handlers" converter does not have any property handler.
        [DecimalPrecision(1)]
        public decimal Score { get; set; }
    }
}
