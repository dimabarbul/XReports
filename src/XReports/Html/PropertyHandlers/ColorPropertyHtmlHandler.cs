using System.Drawing;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="ColorProperty"/> during conversion to HTML.
    /// </summary>
    public class ColorPropertyHtmlHandler : PropertyHandler<ColorProperty, HtmlReportCell>
    {
        /// <inheritdoc />
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
