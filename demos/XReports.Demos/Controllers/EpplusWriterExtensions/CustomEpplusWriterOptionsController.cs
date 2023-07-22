using System;
using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using XReports.Converter;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.Excel.Writers;
using XReports.ReportCellProperties;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.Controllers.EpplusWriterExtensions;

public class CustomEpplusWriterOptionsController : Controller
{
    private const int RecordsCount = 10;

    public IActionResult Index()
    {
        return this.View();
    }

    public IActionResult Download()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
        return this.File(excelStream, Constants.ContentTypeExcel, "CustomEpplusWriterOptions.xlsx");
    }

    private IReportTable<ReportCell> BuildReport()
    {
        ReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddColumn("Name", e => e.Name);
        reportBuilder.AddColumn("Score", e => e.Score)
            .AddProperties(new DecimalPrecisionProperty(2));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
        return reportTable;
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<ExcelReportCell> excelConverter = new(new[]
        {
            new DecimalPrecisionPropertyExcelHandler(),
        });

        return excelConverter.Convert(reportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        EpplusWriter writer = new(
            Options.Create(new EpplusWriterOptions()
            {
                WorksheetName = "Scores",
                StartColumn = 2,
                StartRow = 2,
            }),
            Array.Empty<IEpplusFormatter>());

        return writer.WriteToStream(reportTable);
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
        public string Name { get; set; }

        public decimal Score { get; set; }
    }
}
