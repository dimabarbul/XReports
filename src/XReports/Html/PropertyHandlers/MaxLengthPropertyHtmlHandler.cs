using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="MaxLengthProperty"/> during conversion to HTML.
    /// </summary>
    public class MaxLengthPropertyHtmlHandler : PropertyHandler<MaxLengthProperty, HtmlReportCell>
    {
        /// <inheritdoc />
        public override int Priority => 1;

        /// <inheritdoc />
        protected override void HandleProperty(MaxLengthProperty property, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length <= property.MaxLength)
            {
                return;
            }

            cell.SetValue(value.Substring(0, property.MaxLength - property.Text.Length) + property.Text);
        }
    }
}
