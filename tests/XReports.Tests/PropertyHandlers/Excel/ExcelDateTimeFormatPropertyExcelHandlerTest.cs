using System;
using FluentAssertions;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Excel
{
    public class ExcelDateTimeFormatPropertyExcelHandlerTest
    {
        private const string CSharpFormat = "O";
        private const string ExcelFormat = "YYYY-MM-DD\\THH:MM:SS";

        [Fact]
        public void HandleShouldThrowWhenValueIsNotDateTime()
        {
            ExcelDateTimeFormatPropertyExcelHandler handler = new ExcelDateTimeFormatPropertyExcelHandler();
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(CSharpFormat, ExcelFormat);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            ExcelDateTimeFormatPropertyExcelHandler handler = new ExcelDateTimeFormatPropertyExcelHandler();
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(CSharpFormat, ExcelFormat);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue<string>(null);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().BeNull();
            cell.NumberFormat.Should().Be(ExcelFormat);
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsConvertibleToDateTime()
        {
            ExcelDateTimeFormatPropertyExcelHandler handler = new ExcelDateTimeFormatPropertyExcelHandler();
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(CSharpFormat, ExcelFormat);
            ExcelReportCell cell = new ExcelReportCell();
            cell.SetValue("2023-01-01");

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<DateTime>().Should().Be(new DateTime(2023, 1, 1));
            cell.NumberFormat.Should().Be(ExcelFormat);
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsDateTimeOffset()
        {
            ExcelDateTimeFormatPropertyExcelHandler handler = new ExcelDateTimeFormatPropertyExcelHandler();
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(CSharpFormat, ExcelFormat);
            ExcelReportCell cell = new ExcelReportCell();
            DateTimeOffset now = DateTimeOffset.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetUnderlyingValue().GetType().Should().Be<DateTime>();
            cell.GetValue<DateTime>().Should().Be(now.DateTime);
            cell.NumberFormat.Should().Be(ExcelFormat);
        }

        [Theory]
        [InlineData("yyyy/MM/dd HH:mm:ss")]
        [InlineData("\"Started on \"MMM d, yyyy\" at \"HH:mm AM/PM")]
        public void HandleShouldApplyFormat(string format)
        {
            ExcelDateTimeFormatPropertyExcelHandler handler = new ExcelDateTimeFormatPropertyExcelHandler();
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(CSharpFormat, format);
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
