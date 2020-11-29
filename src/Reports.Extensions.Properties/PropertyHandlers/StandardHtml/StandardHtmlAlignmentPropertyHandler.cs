using Reports.Enums;
using Reports.Html.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.StandardHtml
{
    public class StandardHtmlAlignmentPropertyHandler : SingleTypePropertyHandler<AlignmentProperty, HtmlReportCell>
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
