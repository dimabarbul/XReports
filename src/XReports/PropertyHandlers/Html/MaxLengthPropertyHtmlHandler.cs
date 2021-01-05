using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class MaxLengthPropertyHtmlHandler : PropertyHandler<MaxLengthProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(MaxLengthProperty property, HtmlReportCell cell)
        {
            if (string.IsNullOrEmpty(cell.Value))
            {
                return;
            }

            if (cell.Value.Length <= property.MaxLength)
            {
                return;
            }

            cell.Value = cell.Value.Substring(0, property.MaxLength - 1) + "…";
        }
    }
}
