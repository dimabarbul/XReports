using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
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
