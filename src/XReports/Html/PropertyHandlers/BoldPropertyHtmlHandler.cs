using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="BoldProperty"/> during conversion to HTML.
    /// </summary>
    public class BoldPropertyHtmlHandler : PropertyHandler<BoldProperty, HtmlReportCell>
    {
        /// <inheritdoc />
        protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            cell.SetValue($"<strong>{value}</strong>");
            cell.IsHtml = true;
        }
    }
}
