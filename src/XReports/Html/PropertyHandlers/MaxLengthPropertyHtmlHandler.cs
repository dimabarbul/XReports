using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    public class MaxLengthPropertyHtmlHandler : PropertyHandler<MaxLengthProperty, HtmlReportCell>
    {
        public override int Priority => (int)HtmlPropertyHandlerPriority.Text;

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
