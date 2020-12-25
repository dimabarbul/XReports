using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Html
{
    public class DecimalFormatPropertyHtmlHandler : PropertyHandler<DecimalFormatProperty, HtmlReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, HtmlReportCell cell)
        {
            cell.Html = cell.GetNullableValue<decimal>()?.ToString($"F{property.Precision}");
        }
    }
}
