using Reports.Core.Enums;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Html
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
