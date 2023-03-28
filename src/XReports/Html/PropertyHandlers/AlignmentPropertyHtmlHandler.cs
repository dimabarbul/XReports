using System.Collections.Generic;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    public class AlignmentPropertyHtmlHandler : PropertyHandler<AlignmentProperty, HtmlReportCell>
    {
        private static readonly Dictionary<Alignment, string> Alignments = new Dictionary<Alignment, string>()
        {
            [Alignment.Center] = "center",
            [Alignment.Left] = "left",
            [Alignment.Right] = "right",
        };

        protected override void HandleProperty(AlignmentProperty property, HtmlReportCell cell)
        {
            cell.Styles.Add("text-align", GetAlignmentString(property.Alignment));
        }

        private static string GetAlignmentString(Alignment alignment)
        {
            return Alignments[alignment];
        }
    }
}
