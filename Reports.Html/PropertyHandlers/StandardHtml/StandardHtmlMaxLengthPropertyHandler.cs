using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Models.Properties;
using Reports.PropertyHandlers;

namespace Reports.Html.PropertyHandlers.StandardHtml
{
    public class StandardHtmlMaxLengthPropertyHandler : PropertyHandler<MaxLengthProperty, HtmlReportCell>
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
