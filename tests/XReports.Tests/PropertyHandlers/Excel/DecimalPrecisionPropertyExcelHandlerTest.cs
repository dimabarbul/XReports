using System;
using FluentAssertions;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.ReportCellProperties;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Excel
{
    public class DecimalPrecisionPropertyExcelHandlerTest
    {
        [Fact]
        public void HandleShouldThrowWhenValueIsNotConvertibleToDecimal()
        {
            DecimalPrecisionPropertyExcelHandler handler = new DecimalPrecisionPropertyExcelHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            DecimalPrecisionPropertyExcelHandler handler = new DecimalPrecisionPropertyExcelHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue<string>(null);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetNullableValue<decimal>().Should().BeNull();
            cell.NumberFormat.Should().Be("0.00");
        }

        [Fact]
        public void HandleShouldDisplayTrailingZerosWhenTrailingZerosArePreserved()
        {
            DecimalPrecisionPropertyExcelHandler handler = new DecimalPrecisionPropertyExcelHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(1.23);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(1.23m);
            cell.NumberFormat.Should().Be("0.00");
        }

        [Fact]
        public void HandleShouldNotDisplayTrailingZerosWhenTrailingZerosAreNotPreserved()
        {
            DecimalPrecisionPropertyExcelHandler handler = new DecimalPrecisionPropertyExcelHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2, false);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue(1.23);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<decimal>().Should().Be(1.23m);
            cell.NumberFormat.Should().Be("0.##");
        }
    }
}
