using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Html
{
    public class DecimalFormatPropertyHtmlHandler : PropertyHandler<DecimalFormatProperty, HtmlReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, HtmlReportCell cell)
        {
            cell.Html = cell.GetValue<decimal>().ToString(GetFormatString(property));
        }

        private string GetFormatString(DecimalFormatProperty property)
        {
            string formatSpecifier = property.CultureAgnostic ? "F" : "N";

            return $"{formatSpecifier}{property.Precision}";
        }
    }
}
