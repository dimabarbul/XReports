using System;
using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using XReports.Converter;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.MVC.Controllers.EpplusWriterExtensions
{
    public class BorderController : Controller
    {
        private const int RecordsCount = 20;

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Download()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

            Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Border.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            EpplusWriter writer = new BorderExcelWriter();

            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ReportSchemaBuilder<Entity> reportBuilder = new ReportSchemaBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name);
            reportBuilder.AddColumn("Last Score", e => e.LastScore);
            reportBuilder.AddColumn("Score", e => e.Score);

            IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(Array.Empty<IPropertyHandler<ExcelReportCell>>());

            return excelConverter.Convert(reportTable);
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.LastScore, f => f.Random.Int(1, 10))
                .RuleFor(e => e.Score, f => Math.Round(f.Random.Decimal(0, 100), 2))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }

            public int LastScore { get; set; }

            public decimal Score { get; set; }
        }

        private class BorderExcelWriter : EpplusWriter
        {
            protected override void PostCreate(ExcelWorksheet worksheet, ExcelAddress headerAddress, ExcelAddress bodyAddress)
            {
                base.PostCreate(worksheet, headerAddress, bodyAddress);

                // Border around each header cell.
                Border headerBorder = worksheet.Cells[headerAddress.Address].Style.Border;
                headerBorder.Bottom.Style = headerBorder.Left.Style =
                    headerBorder.Top.Style = headerBorder.Right.Style = ExcelBorderStyle.Thin;

                // Border around all body cells.
                worksheet.Cells[bodyAddress.Address].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }
    }
}
