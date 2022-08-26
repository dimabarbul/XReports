using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class BoldPropertyHtmlHandler : PropertyHandler<BoldProperty, HtmlReportCell>
    {
        public override int Priority => (int)HtmlPropertyHandlerPriority.Html;

        protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            cell.SetValue($"<strong>{value}</strong>");
            cell.IsHtml = true;
        }
    }
}
