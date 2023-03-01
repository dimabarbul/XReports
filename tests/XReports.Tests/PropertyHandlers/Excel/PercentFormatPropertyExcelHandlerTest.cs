using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Excel
{
    public class PercentFormatPropertyExcelHandlerTest
    {
        [Fact]
        public void HandleShouldThrowWhenValueIsNotConvertibleToDecimal()
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(3);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(1);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue<string>(null);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetNullableValue<decimal>().Should().BeNull();
            cell.NumberFormat.Should().Be("0.0%");
        }

        [Fact]
        public void HandleShouldRoundAwayFromZeroWhenValueHasMoreDecimalDigits()
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(1);
            ExcelReportCell cell = new ExcelReportCell();
            // Important fact about -1.2345 is that it is differently rounded to 3 decimal
            // places (before multiplying by 100) using away-from-zero midpoint rounding
            // type than using to-even or to-zero.
            cell.SetValue(-1.2345m);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(-1.2345m);
            cell.NumberFormat.Should().Be("0.0%");
        }

        [Theory]
        [InlineData(0.11)]
        [InlineData(0.11001)]
        [InlineData(0.10999)]
        public void HandleShouldDisplayTrailingZerosWhenTrailingZerosArePreserved(decimal value)
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(1);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(value);
            cell.NumberFormat.Should().Be("0.0%");
        }

        [Theory]
        [InlineData(0.11)]
        [InlineData(0.11001)]
        [InlineData(0.10999)]
        public void HandleShouldNotDisplayTrailingZerosWhenTrailingZerosAreNotPreserved(decimal value)
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, preserveTrailingZeros: false);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(value);
            cell.NumberFormat.Should().Be("0.##%");
        }

        [Fact]
        public void HandleShouldConsiderCurrentCulture()
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(2);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(0.1);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(0.1m);
            cell.NumberFormat.Should().Be("0.00%");
        }

        [Fact]
        public void HandleShouldAddPostfixTextWhenPostfixTextIsCustom()
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, postfixText: " (%)");
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(0.1234);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(0.1234m);
            cell.NumberFormat.Should().Be("0.00\" (\"%\")\"");
        }

        [Fact]
        public void HandleShouldMultiplyValueWhenPostfixTextDoesNotContainPercentSign()
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, postfixText: string.Empty);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(0.1234);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(12.34m);
            cell.NumberFormat.Should().Be("0.00");
        }

        [Theory]
        [InlineData(" (\"percent\")", "\" (\"\\\"\"percent\"\\\"\")\"")]
        [InlineData(" (\"%\")", "\" (\"\\\"\"\"%\"\"\\\"\")\"")]
        [InlineData("\"%\"", "\"\"\\\"\"\"%\"\"\\\"\"\"")]
        [InlineData(" - %", "\" - \"%")]
        [InlineData("%", "%")]
        [InlineData("% - \"percent\"", "%\" - \"\\\"\"percent\"\\\"\"\"")]
        public void HandleShouldEscapeQuotesAndPercent(string postfixText, string formattedPostfixText)
        {
            PercentFormatPropertyExcelHandler handler = new PercentFormatPropertyExcelHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, postfixText: postfixText);
            ExcelReportCell cell = new ExcelReportCell();
            decimal value = 0.1234m;
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            decimal expected = postfixText.Contains('%', StringComparison.Ordinal) ?
                value :
                value * 100;
            cell.GetValue<decimal>().Should().Be(expected);
            cell.NumberFormat.Should().Be($"0.00{formattedPostfixText}");
        }
    }
}
