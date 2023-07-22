using System.IO;
using System.Linq;
using FluentAssertions;
using OfficeOpenXml;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Table;
using Xunit;

namespace XReports.Tests.Excel.Writers.EpplusWriterTests
{
    public class WriteToWorksheetTest
    {
        [Fact]
        public void WriteToWorksheetShouldUseProvidedWorksheetAtSpecifiedPosition()
        {
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Workbook.Worksheets.Add("Test");
            IReportTable<ExcelReportCell> excelReport = Helper.CreateExcelReport();
            IEpplusWriter writer = new EpplusWriter();

            ExcelAddress excelAddress = writer.WriteToWorksheet(excelReport, excelPackage.Workbook.Worksheets.First(), 2, 4);

            excelAddress.Address.Should().Be("D2:E4");
            excelPackage.Workbook.Worksheets.Should().HaveCount(1);
            excelPackage.Workbook.Worksheets.First().Cells[2, 4, 4, 5]
                .Select(c => c.Value?.ToString())
                .Should()
                .Equal(Helper.GetFlattenedReportValues());
        }
    }
}
