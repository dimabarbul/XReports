using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class DecimalFormatPropertyHtmlHandler : PropertyHandler<DecimalFormatProperty, HtmlReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, HtmlReportCell cell)
        {
            cell.Value = cell.GetNullableValue<decimal>()?.ToString($"F{property.Precision}");
        }
    }
}
