using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="ColorProperty"/> during conversion to Excel.
    /// </summary>
    public class ColorPropertyExcelHandler : PropertyHandler<ColorProperty, ExcelReportCell>
    {
        /// <inheritdoc />
        protected override void HandleProperty(ColorProperty property, ExcelReportCell cell)
        {
            cell.FontColor = property.FontColor;
            cell.BackgroundColor = property.BackgroundColor;
        }
    }
}
