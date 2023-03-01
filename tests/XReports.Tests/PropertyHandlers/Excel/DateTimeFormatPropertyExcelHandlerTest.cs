using System;
using FluentAssertions;
using XReports.Models;
using XReports.Properties;
using XReports.Properties.Excel;
using XReports.PropertyHandlers.Excel;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Excel
{
    public class DateTimeFormatPropertyExcelHandlerTest
    {
        [Fact]
        public void HandleShouldThrowWhenValueIsNotDateTime()
        {
            DateTimeFormatPropertyExcelHandler handler = new DateTimeFormatPropertyExcelHandler();
            DateTimeFormatProperty property = new DateTimeFormatProperty("O");
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            DateTimeFormatPropertyExcelHandler handler = new DateTimeFormatPropertyExcelHandler();
            const string format = "O";
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue<string>(null);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().BeNull();
            cell.NumberFormat.Should().Be(format);
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsConvertibleToDateTime()
        {
            DateTimeFormatPropertyExcelHandler handler = new DateTimeFormatPropertyExcelHandler();
            const string format = "O";
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue("2023-01-01");

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<DateTime>().Should().Be(new DateTime(2023, 1, 1));
            cell.NumberFormat.Should().Be(format);
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsDateTimeOffset()
        {
            DateTimeFormatPropertyExcelHandler handler = new DateTimeFormatPropertyExcelHandler();
            const string format = "O";
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            ExcelReportCell cell = new ExcelReportCell();
            DateTimeOffset now = DateTimeOffset.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetUnderlyingValue().GetType().Should().Be<DateTime>();
            cell.GetValue<DateTime>().Should().Be(now.DateTime);
            cell.NumberFormat.Should().Be(format);
        }

        [Theory]
        [InlineData("O")]
        [InlineData("yyyy/MM/dd HH:mm:ss")]
        [InlineData("\"Started on \"MMM d, yyyy 'at' HH:mm AM/PM")]
        public void HandleShouldApplyFormat(string format)
        {
            DateTimeFormatPropertyExcelHandler handler = new DateTimeFormatPropertyExcelHandler();
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            ExcelReportCell cell = new ExcelReportCell();
            DateTime now = DateTime.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<DateTime>().Should().Be(now);
            cell.NumberFormat.Should().Be(format);
        }

        [Fact]
        public void HandleShouldTakeFirstFormatWhenPropertyIsExcelDateTimeFormatProperty()
        {
            DateTimeFormatPropertyExcelHandler handler = new DateTimeFormatPropertyExcelHandler();
            const string format = "O";
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(format, "YYYY/MM/DD HH:MM:SS");
            ExcelReportCell cell = new ExcelReportCell();
            DateTime now = DateTime.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<DateTime>().Should().Be(now);
            cell.NumberFormat.Should().Be(format);
        }
    }
}
