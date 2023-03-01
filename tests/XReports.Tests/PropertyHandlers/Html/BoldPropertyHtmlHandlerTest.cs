using FluentAssertions;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers.Html;
using Xunit;

namespace XReports.Tests.PropertyHandlers.Html
{
    public class BoldPropertyHtmlHandlerTest
    {
        [Fact]
        public void HandleShouldWrapInStrongTag()
        {
            BoldPropertyHtmlHandler handler = new BoldPropertyHtmlHandler();
            BoldProperty property = new BoldProperty();
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue("test");

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeTrue();
            cell.GetValue<string>().Should().Be("<strong>test</strong>");
        }

        [Fact]
        public void HandleShouldConvertToStringWhenValueIsNotString()
        {
            BoldPropertyHtmlHandler handler = new BoldPropertyHtmlHandler();
            BoldProperty property = new BoldProperty();
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue(123);

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeTrue();
            cell.GetValue<string>().Should().Be("<strong>123</strong>");
        }
    }
}
