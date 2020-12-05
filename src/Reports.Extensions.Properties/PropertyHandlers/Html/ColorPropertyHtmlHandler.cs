using System.Drawing;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Html
{
    public class ColorPropertyHtmlHandler : PropertyHandler<ColorProperty, HtmlReportCell>
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
