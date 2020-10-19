using Reports.Html.Enums;
using Reports.Html.Models;
using Reports.Models.Properties;

namespace Reports.Html.PropertyHandlers.StandardHtml
{
    public class StandardHtmlMaxLengthPropertyHandler : HtmlPropertyHandler<MaxLengthProperty>
    {
        public override HtmlPropertyHandlerPriority Priority => HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(MaxLengthProperty property, HtmlReportTableCell cell)
        {
            if (string.IsNullOrEmpty(cell.Text))
            {
                return;
            }

            if (cell.Text.Length <= property.MaxLength)
            {
                return;
            }

            cell.Text = cell.Text.Substring(0, property.MaxLength - 1) + "…";
        }
    }
}
