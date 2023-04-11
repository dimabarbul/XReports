using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="MaxLengthProperty"/> during conversion to Excel.
    /// </summary>
    public class MaxLengthPropertyExcelHandler : PropertyHandler<MaxLengthProperty, ExcelReportCell>
    {
        /// <inheritdoc />
        protected override void HandleProperty(MaxLengthProperty property, ExcelReportCell cell)
        {
            string text = cell.GetValue<string>();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (text.Length <= property.MaxLength)
            {
                return;
            }

            cell.SetValue(text.Substring(0, property.MaxLength - property.Text.Length) + property.Text);
        }
    }
}
