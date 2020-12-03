using Reports.Enums;
using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Html
{
    public class AlignmentPropertyHtmlHandler : PropertyHandler<AlignmentProperty, HtmlReportCell>
    {
        protected override void HandleProperty(AlignmentProperty property, HtmlReportCell cell)
        {
            cell.Styles.Add("text-align", this.GetAlignmentString(property.AlignmentType));
        }

        private string GetAlignmentString(AlignmentType alignmentType)
        {
            return alignmentType.ToString().ToLowerInvariant();
        }
    }
}
