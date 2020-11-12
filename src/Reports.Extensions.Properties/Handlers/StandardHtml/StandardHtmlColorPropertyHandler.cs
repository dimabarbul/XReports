using System.Drawing;
using Reports.Html.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.StandardHtml
{
    public class StandardHtmlColorPropertyHandler : SingleTypePropertyHandler<ColorProperty, HtmlReportCell>
    {
        protected override void HandleProperty(ColorProperty property, HtmlReportCell cell)
        {
            if (property.FontColor != null)
            {
                cell.Styles.Add("color", ColorTranslator.ToHtml(property.FontColor.Value));
            }

            if (property.BackgroundColor != null)
            {
                cell.Styles.Add("background-color", ColorTranslator.ToHtml(property.BackgroundColor.Value));
            }
        }
    }
}
