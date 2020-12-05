using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
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
