using Reports.Core.Enums;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Html
{
    public class MaxLengthPropertyHtmlHandler : PropertyHandler<MaxLengthProperty, HtmlReportCell>
    {
        public override int Priority => (int) HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(MaxLengthProperty property, HtmlReportCell cell)
        {
            if (string.IsNullOrEmpty(cell.Html))
            {
                return;
            }

            if (cell.Html.Length <= property.MaxLength)
            {
                return;
            }

            cell.Html = cell.Html.Substring(0, property.MaxLength - 1) + "…";
        }
    }
}
