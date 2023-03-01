using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using XReports.Models;
using XReports.Properties;
using XReports.Properties.Excel;
using XReports.PropertyHandlers.Html;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Html
{
    public class DateTimeFormatPropertyHtmlHandlerTest
    {
        [Fact]
        public void HandleShouldThrowWhenValueIsNotDateTime()
        {
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            DateTimeFormatProperty property = new DateTimeFormatProperty("O");
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            DateTimeFormatProperty property = new DateTimeFormatProperty("O");
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue<string>(null);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().BeNull();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsConvertibleToDateTimeOffset()
        {
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            const string format = "O";
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue("2023-02-01T12:34:56-14:00");

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be(
                new DateTimeOffset(2023, 2, 1, 12, 34, 56, TimeSpan.FromHours(-14)).
                    LocalDateTime.
                    ToString(format, CultureInfo.CurrentCulture));
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsDateTimeOffset()
        {
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            const string format = "O";
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            HtmlReportCell cell = new HtmlReportCell();
            DateTimeOffset now = DateTimeOffset.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be(now.ToString(format, CultureInfo.CurrentCulture));
        }

        [Theory]
        [InlineData("O")]
        [InlineData("yyyy/MM/dd HH:mm:ss")]
        [InlineData("\"Started on \"MMM d, yyyy 'at' HH:mm tt")]
        public void HandleShouldApplyFormat(string format)
        {
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            HtmlReportCell cell = new HtmlReportCell();
            DateTime now = DateTime.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be(now.ToString(format, CultureInfo.CurrentCulture));
        }

        [Theory]
        [InlineData("O")]
        [InlineData("yyyy/MM/dd HH:mm:ss")]
        [InlineData("\"Started on \"MMM d, yyyy 'at' HH:mm tt")]
        public void HandleShouldConsiderCurrentCulture(string format)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            DateTimeFormatProperty property = new DateTimeFormatProperty(format);
            HtmlReportCell cell = new HtmlReportCell();
            DateTime now = DateTime.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be(now.ToString(format, cultureInfo));
        }

        [Fact]
        public void HandleShouldTakeFirstFormatWhenPropertyIsExcelDateTimeFormatProperty()
        {
            DateTimeFormatPropertyHtmlHandler handler = new DateTimeFormatPropertyHtmlHandler();
            const string format = "O";
            ExcelDateTimeFormatProperty property = new ExcelDateTimeFormatProperty(format, "YYYY/MM/DD HH:MM:SS");
            HtmlReportCell cell = new HtmlReportCell();
            DateTime now = DateTime.Now;
            cell.SetValue(now);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be(now.ToString(format, CultureInfo.CurrentCulture));
        }
    }
}
