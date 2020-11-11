using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Models.Properties;
using Reports.PropertyHandlers;

namespace Reports.Html.PropertyHandlers.StandardHtml
{
    public class StandardHtmlBoldPropertyHandler : SingleTypePropertyHandler<BoldProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Html;

        protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
        {
            cell.Html = $"<strong>{cell.Html}</strong>";
        }
    }
}
