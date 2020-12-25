using Reports.Enums;
using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Html
{
    public class PercentFormatPropertyHtmlHandler : PropertyHandler<PercentFormatProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(PercentFormatProperty property, HtmlReportCell cell)
        {
            cell.Html = (cell.GetNullableValue<decimal>() * 100)?.ToString($"F{property.Precision}") + property.PostfixText;
        }
    }
}
