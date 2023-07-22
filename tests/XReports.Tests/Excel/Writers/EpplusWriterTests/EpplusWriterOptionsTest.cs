using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Table;
using Xunit;

namespace XReports.Tests.Excel.Writers.EpplusWriterTests
{
    public class EpplusWriterOptionsTest
    {
        [Fact]
        public void CreateShouldUseDefaultOptionsWhenWithoutOptions()
        {
            IReportTable<ExcelReportCell> excelReport = Helper.CreateExcelReport();
            EpplusWriter writer = new EpplusWriter();

            Stream stream = writer.WriteToStream(excelReport);

            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Load(stream);
            ExcelWorkbook workbook = excelPackage.Workbook;
            workbook.Worksheets.Should().HaveCount(1);
            ExcelWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name.Should().Be("Data");
            worksheet.Cells[1, 1, 3, 2]
                .Select(c => c.Value?.ToString())
                .Should()
                .Equal(Helper.GetFlattenedReportValues());
            worksheet.Cells[2, 1, 3, 2]
                .Select(c => c.Style.Font.Bold)
                .Should()
                .Equal(
                    false, true,
                    false, true);
        }

        [Fact]
        public void CreateShouldUseOptionsWhenWithOptions()
        {
            IReportTable<ExcelReportCell> excelReport = Helper.CreateExcelReport();
            EpplusWriter writer = new EpplusWriter(
                Options.Create(new EpplusWriterOptions()
                {
                    WorksheetName = "Test",
                    StartColumn = 2,
                    StartRow = 2,
                }),
                Array.Empty<IEpplusFormatter>());

            Stream stream = writer.WriteToStream(excelReport);

            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Load(stream);
            ExcelWorkbook workbook = excelPackage.Workbook;
            workbook.Worksheets.Should().HaveCount(1);
            ExcelWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name.Should().Be("Test");
            worksheet.Cells[2, 2, 4, 3]
                .Select(c => c.Value?.ToString())
                .Should()
                .Equal(Helper.GetFlattenedReportValues());
            worksheet.Cells[3, 2, 4, 3]
                .Select(c => c.Style.Font.Bold)
                .Should()
                .Equal(
                    false, true,
                    false, true);
        }
    }
}
