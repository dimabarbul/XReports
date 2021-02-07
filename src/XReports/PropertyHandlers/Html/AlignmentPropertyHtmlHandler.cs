using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class AlignmentPropertyHtmlHandler : PropertyHandler<AlignmentProperty, HtmlReportCell>
    {
        protected override void HandleProperty(AlignmentProperty property, HtmlReportCell cell)
        {
            cell.Styles.Add("text-align", this.GetAlignmentString(property.Alignment));
        }

        private string GetAlignmentString(Alignment alignment)
        {
            return alignment.ToString().ToLowerInvariant();
        }
    }
}
