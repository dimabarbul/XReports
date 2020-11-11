using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.StandardHtml
{
    public class StandardHtmlMaxLengthPropertyHandler : SingleTypePropertyHandler<MaxLengthProperty, HtmlReportCell>
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
