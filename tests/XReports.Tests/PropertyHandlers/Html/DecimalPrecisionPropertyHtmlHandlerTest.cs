using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers.Html;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Html
{
    public class DecimalPrecisionPropertyHtmlHandlerTest
    {
        [Fact]
        public void HandleShouldThrowWhenValueIsNotConvertibleToDecimal()
        {
            DecimalPrecisionPropertyHtmlHandler handler = new DecimalPrecisionPropertyHtmlHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(3);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            DecimalPrecisionPropertyHtmlHandler handler = new DecimalPrecisionPropertyHtmlHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(3);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue<string>(null);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().BeNull();
        }

        [Fact]
        public void HandleShouldRoundAwayFromZeroWhenValueHasMoreDecimalDigits()
        {
            DecimalPrecisionPropertyHtmlHandler handler = new DecimalPrecisionPropertyHtmlHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(3);
            HtmlReportCell cell = new HtmlReportCell();
            // Important fact about -1.2345 is that it is differently rounded to 3 decimal
            // places using away-from-zero midpoint rounding type than using to-even or to-zero.
            cell.SetValue(-1.2345m);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("-1.235");
        }

        [Theory]
        [InlineData(1.1)]
        [InlineData(1.1001)]
        [InlineData(1.0999)]
        public void HandleShouldDisplayTrailingZerosWhenTrailingZerosArePreserved(decimal value)
        {
            DecimalPrecisionPropertyHtmlHandler handler = new DecimalPrecisionPropertyHtmlHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("1.10");
        }

        [Theory]
        [InlineData(1.1)]
        [InlineData(1.1001)]
        [InlineData(1.0999)]
        public void HandleShouldNotDisplayTrailingZerosWhenTrailingZerosAreNotPreserved(decimal value)
        {
            DecimalPrecisionPropertyHtmlHandler handler = new DecimalPrecisionPropertyHtmlHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2, false);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("1.1");
        }

        [Fact]
        public void HandleShouldConsiderCurrentCulture()
        {
            DecimalPrecisionPropertyHtmlHandler handler = new DecimalPrecisionPropertyHtmlHandler();
            DecimalPrecisionProperty property = new DecimalPrecisionProperty(2);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(1100);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("1100,00");
        }
    }
}
