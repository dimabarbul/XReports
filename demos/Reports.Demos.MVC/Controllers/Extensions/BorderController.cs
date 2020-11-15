using System;
using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reports.Builders;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Demos.MVC.Controllers.Extensions
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Custom format.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            EpplusWriter writer = new BorderExcelWriter();

            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            VerticalReportBuilder<Entity> reportBuilder = new VerticalReportBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name);
            reportBuilder.AddColumn("Last Score", e => e.LastScore);
            reportBuilder.AddColumn("Score", e => e.Score);

            IReportTable<ReportCell> reportTable = reportBuilder.Build(this.GetData());
            return reportTable;
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
            });

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
            protected override void FormatHeader(ExcelWorksheet worksheet, ExcelAddress headerAddress)
            {
                base.FormatHeader(worksheet, headerAddress);

                Border bodyBorder = worksheet.Cells[headerAddress.Address].Style.Border;
                bodyBorder.Bottom.Style = bodyBorder.Left.Style =
                    bodyBorder.Top.Style = bodyBorder.Right.Style = ExcelBorderStyle.Thin;
            }

            protected override ExcelAddress WriteBody(ExcelWorksheet worksheet, IReportTable<ExcelReportCell> table)
            {
                ExcelAddress bodyAddress = base.WriteBody(worksheet, table);

                worksheet.Cells[bodyAddress.Address].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                return bodyAddress;
            }
        }
    }
}
