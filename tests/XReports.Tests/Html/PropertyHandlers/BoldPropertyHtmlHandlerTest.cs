using FluentAssertions;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.ReportCellProperties;
using Xunit;

namespace XReports.Tests.Html.PropertyHandlers
{
    public class BoldPropertyHtmlHandlerTest
    {
        [Fact]
        public void HandleShouldAddCorrectStyle()
        {
            BoldPropertyHtmlHandler handler = new BoldPropertyHtmlHandler();
            BoldProperty property = new BoldProperty();
            HtmlReportCell cell = new HtmlReportCell();
            cell.SetValue("test");

            bool handled = handler.Handle(property, cell);

            handled.Should().BeTrue();
            cell.IsHtml.Should().BeFalse();
            cell.GetValue<string>().Should().Be("test");
            cell.Styles.Should().ContainKey("font-weight")
                .WhichValue.Should().Be("bold");
        }
    }
}
