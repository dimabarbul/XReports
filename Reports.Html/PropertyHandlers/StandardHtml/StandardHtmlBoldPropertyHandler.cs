using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Models.Properties;

namespace Reports.Html.PropertyHandlers.StandardHtml
{
    public class StandardHtmlBoldPropertyHandler : HtmlPropertyHandler<BoldProperty>
    {
        public override HtmlPropertyHandlerPriority Priority => HtmlPropertyHandlerPriority.Html;

        protected override void HandleProperty(BoldProperty property, HtmlReportTableCell cell)
        {
            cell.Html = $"<strong>{cell.Html ?? cell.Text}</strong>";
        }
    }
}
