using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Models.Properties;

namespace Reports.Html.PropertyHandlers.StandardHtml
{
    public class StandardHtmlBoldPropertyHandler : HtmlPropertyHandle<BoldProperty>
    {
        protected override void HandleProperty(BoldProperty property, HtmlReportTableCell cell)
        {
            cell.Html = $"<strong>{(cell.Html ?? cell.Text)}</strong>";
        }
    }
}
