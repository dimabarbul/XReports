using Reports.Enums;
using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Html
{
    public class BoldPropertyHtmlHandler : PropertyHandler<BoldProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Html;

        protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
        {
            cell.Html = $"<strong>{cell.Html}</strong>";
        }
    }
}
