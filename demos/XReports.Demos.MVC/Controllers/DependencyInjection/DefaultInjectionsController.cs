using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Converter;
using XReports.Demos.MVC.Models.Shared;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.Writers;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.Demos.MVC.Controllers.DependencyInjection;

public class DefaultInjectionsController : Controller
{
    private const int RecordsCount = 10;

    private readonly IAttributeBasedBuilder attributeBasedBuilder;
    private readonly IReportConverter<HtmlReportCell> htmlConverter;
    private readonly IReportConverter<ExcelReportCell> excelConverter;
    private readonly IHtmlStringWriter htmlWriter;
    private readonly IEpplusWriter excelWriter;

    public DefaultInjectionsController(
        IAttributeBasedBuilder attributeBasedBuilder,
        IReportConverter<HtmlReportCell> htmlConverter,
        IReportConverter<ExcelReportCell> excelConverter,
        IHtmlStringWriter htmlWriter,
        IEpplusWriter excelWriter)
    {
        this.attributeBasedBuilder = attributeBasedBuilder;
        this.htmlConverter = htmlConverter;
        this.excelConverter = excelConverter;
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

        return this.File(excelStream, Constants.ContentTypeExcel, "DefaultInjections.xlsx");
    }

    private string GetReportHtml()
    {
        IReportTable<ReportCell> reportTable = this.attributeBasedBuilder.BuildSchema<Entity>()
            .BuildReportTable(this.GetData());
        IReportTable<HtmlReportCell> htmlReportTable = this.htmlConverter.Convert(reportTable);
        string html = this.htmlWriter.WriteToString(htmlReportTable);

        return html;
    }

    private Stream GetExcelStream()
    {
        IReportTable<ReportCell> reportTable = this.attributeBasedBuilder.BuildSchema<Entity>()
            .BuildReportTable(this.GetData());
        IReportTable<ExcelReportCell> excelReportTable = this.excelConverter.Convert(reportTable);
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
        [DecimalPrecision(1)]
        public decimal Score { get; set; }
    }
}
