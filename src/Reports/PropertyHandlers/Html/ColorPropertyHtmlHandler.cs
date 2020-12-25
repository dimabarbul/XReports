using System.Drawing;
using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Html
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
