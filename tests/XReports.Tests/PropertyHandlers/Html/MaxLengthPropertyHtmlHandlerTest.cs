using FluentAssertions;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.ReportCellProperties;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Html
{
    public class MaxLengthPropertyHtmlHandlerTest
    {
        [Fact]
        public void HandleShouldNotChangeValueWhenLengthIsMaxLength()
        {
            MaxLengthPropertyHtmlHandler handler = new MaxLengthPropertyHtmlHandler();
            MaxLengthProperty property = new MaxLengthProperty(5);
            HtmlReportCell cell = new HtmlReportCell();
            string value = "Short";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be(value);
        }

        [Fact]
        public void HandleShouldChangeValueWhenLengthIsGreaterThanMaxLength()
        {
            MaxLengthPropertyHtmlHandler handler = new MaxLengthPropertyHtmlHandler();
            MaxLengthProperty property = new MaxLengthProperty(5);
            HtmlReportCell cell = new HtmlReportCell();
            string value = "Very long";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be("Very…");
        }

        [Fact]
        public void HandleShouldChangeValueWhenLengthIsGreaterThanMaxLengthAndCustomPropertyText()
        {
            MaxLengthPropertyHtmlHandler handler = new MaxLengthPropertyHtmlHandler();
            MaxLengthProperty property = new MaxLengthProperty(5, "...");
            HtmlReportCell cell = new HtmlReportCell();
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
            MaxLengthPropertyHtmlHandler handler = new MaxLengthPropertyHtmlHandler();
            MaxLengthProperty property = new MaxLengthProperty(5, text);
            HtmlReportCell cell = new HtmlReportCell();
            string value = "Very long";
            cell.SetValue(value);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.GetValue<string>().Should().Be("Very ");
        }
    }
}
