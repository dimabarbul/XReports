using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class DecimalPrecisionPropertyHtmlHandler : PropertyHandler<DecimalPrecisionProperty, HtmlReportCell>
    {
        protected override void HandleProperty(DecimalPrecisionProperty property, HtmlReportCell cell)
        {
            cell.SetValue(cell.GetNullableValue<decimal>()?.ToString($"F{property.Precision}"));
        }
    }
}
