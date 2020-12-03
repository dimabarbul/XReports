using Reports.Enums;
using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Html
{
    public class PercentFormatPropertyHtmlHandler : PropertyHandler<PercentFormatProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(PercentFormatProperty property, HtmlReportCell cell)
        {
            cell.Html = (cell.GetValue<decimal>() * 100).ToString($"F{property.Precision}") + " %";
        }
    }
}
