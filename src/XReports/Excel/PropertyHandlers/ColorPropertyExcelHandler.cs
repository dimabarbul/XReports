using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    public class ColorPropertyExcelHandler : PropertyHandler<ColorProperty, ExcelReportCell>
    {
        protected override void HandleProperty(ColorProperty property, ExcelReportCell cell)
        {
            cell.FontColor = property.FontColor;
            cell.BackgroundColor = property.BackgroundColor;
        }
    }
}
