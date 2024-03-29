using FluentAssertions;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.ReportCellProperties;
using Xunit;

namespace XReports.Tests.Excel.PropertyHandlers
{
    public class MaxLengthPropertyExcelHandlerTest
    {
        [Fact]
        public void HandleShouldNotChangeValueWhenLengthIsMaxLength()
        {
            MaxLengthPropertyExcelHandler handler = new MaxLengthPropertyExcelHandler();
            MaxLengthProperty property = new MaxLengthProperty(5);
            ExcelReportCell cell = new ExcelReportCell();
            string value = "Short";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be(value);
        }

        [Fact]
        public void HandleShouldChangeValueWhenLengthIsGreaterThanMaxLength()
        {
            MaxLengthPropertyExcelHandler handler = new MaxLengthPropertyExcelHandler();
            MaxLengthProperty property = new MaxLengthProperty(5);
            ExcelReportCell cell = new ExcelReportCell();
            string value = "Very long";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be("Very…");
        }

        [Fact]
        public void HandleShouldChangeValueWhenLengthIsGreaterThanMaxLengthAndCustomPropertyText()
        {
            MaxLengthPropertyExcelHandler handler = new MaxLengthPropertyExcelHandler();
            MaxLengthProperty property = new MaxLengthProperty(5, "...");
            ExcelReportCell cell = new ExcelReportCell();
            string value = "Very long";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be("Ve...");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void HandleShouldChangeValueWhenLengthIsGreaterThanMaxLengthAndCustomPropertyTextIsEmpty(string text)
        {
            MaxLengthPropertyExcelHandler handler = new MaxLengthPropertyExcelHandler();
            MaxLengthProperty property = new MaxLengthProperty(5, text);
            ExcelReportCell cell = new ExcelReportCell();
            string value = "Very long";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be("Very ");
        }
    }
}
