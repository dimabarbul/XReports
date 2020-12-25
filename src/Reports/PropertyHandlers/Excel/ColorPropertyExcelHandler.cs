using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Excel
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
