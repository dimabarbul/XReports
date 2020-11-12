using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.StandardHtml
{
    public class StandardHtmlPercentFormatPropertyHandler : SingleTypePropertyHandler<PercentFormatProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(PercentFormatProperty property, HtmlReportCell cell)
        {
            cell.Html = (cell.GetValue<decimal>() * 100).ToString($"F{property.Precision}") + " %";
        }
    }
}
