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
    public class WriteToStreamTest
    {
        [Fact]
        public void WriteToStreamShouldCreateAndRewindStreamWhenWritingToNewStream()
        {
            IReportTable<ExcelReportCell> excelReport = Helper.CreateExcelReport();
            IEpplusWriter writer = new EpplusWriter();

            Stream stream = writer.WriteToStream(excelReport);

            stream.Position.Should().Be(0);
            stream.Length.Should().BeGreaterThan(0);
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Load(stream);
            excelPackage.Workbook.Worksheets.Should().HaveCount(1);
            excelPackage.Workbook.Worksheets.First().Cells[1, 1, 3, 2]
                .Select(c => c.Value?.ToString())
                .Should()
                .Equal(Helper.GetFlattenedReportValues());
        }

        [Fact]
        public void WriteToStreamShouldAppendAndNotRewindStreamWhenWritingToExistingStream()
        {
            const string initialData = "test";
            IReportTable<ExcelReportCell> excelReport = Helper.CreateExcelReport();
            IEpplusWriter writer = new EpplusWriter();
            MemoryStream stream = new MemoryStream();
            new StreamWriter(stream).Write(initialData);

            writer.WriteToStream(excelReport, stream);

            stream.Length.Should().BeGreaterThan(0);
            stream.Position.Should().Be(stream.Length);
            stream.Seek(initialData.Length, SeekOrigin.Begin);
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Load(stream);
            excelPackage.Workbook.Worksheets.Should().HaveCount(1);
            excelPackage.Workbook.Worksheets.First().Cells[1, 1, 3, 2]
                .Select(c => c.Value?.ToString())
                .Should()
                .Equal(Helper.GetFlattenedReportValues());
        }
    }
}
