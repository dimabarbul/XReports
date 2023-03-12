using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.ReportCellProperties;
using Xunit;

namespace XReports.Tests.Html.PropertyHandlers
{
    public class PercentFormatPropertyHtmlHandlerTest
    {
        [Fact]
        public void HandleShouldThrowWhenValueIsNotConvertibleToDecimal()
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(3);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue("test");

            Action action = () => handler.Handle(property, cell);

            action.Should().ThrowExactly<FormatException>();
        }

        [Fact]
        public void HandleShouldWorkWhenValueIsNull()
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(1);
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
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(1);
            HtmlReportCell cell = new HtmlReportCell();
            // Important fact about -1.2345 is that it is differently rounded to 3 decimal
            // places (before multiplying by 100) using away-from-zero midpoint rounding
            // type than using to-even or to-zero.
            cell.SetValue(-1.2345m);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("-123.5%");
        }

        [Theory]
        [InlineData(0.11)]
        [InlineData(0.11001)]
        [InlineData(0.10999)]
        public void HandleShouldDisplayTrailingZerosWhenTrailingZerosArePreserved(decimal value)
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(1);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("11.0%");
        }

        [Theory]
        [InlineData(0.11)]
        [InlineData(0.11001)]
        [InlineData(0.10999)]
        public void HandleShouldNotDisplayTrailingZerosWhenTrailingZerosAreNotPreserved(decimal value)
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, preserveTrailingZeros: false);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("11%");
        }

        [Fact]
        public void HandleShouldConsiderCurrentCulture()
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(2);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(0.1);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("10,00%");
        }

        [Fact]
        public void HandleShouldAddPostfixTextWhenPostfixTextIsCustom()
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, postfixText: " (%)");
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(0.1234);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("12.34 (%)");
        }

        [Fact]
        public void HandleShouldAddPostfixTextWhenPostfixTextDoesNotContainPercentSign()
        {
            PercentFormatPropertyHtmlHandler handler = new PercentFormatPropertyHtmlHandler();
            PercentFormatProperty property = new PercentFormatProperty(2, postfixText: string.Empty);
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(0.1234);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("12.34");
        }
    }
}
