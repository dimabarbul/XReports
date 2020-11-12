using Reports.Html.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.StandardHtml
{
    public class StandardHtmlDecimalFormatPropertyHandler : SingleTypePropertyHandler<DecimalFormatProperty, HtmlReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, HtmlReportCell cell)
        {
            cell.Html = cell.GetValue<decimal>().ToString($"F{property.Precision}");
        }
    }
}
