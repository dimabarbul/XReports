using Reports.Excel.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class ExcelColorPropertyHandler : SingleTypePropertyHandler<ColorProperty, ExcelReportCell>
    {
        protected override void HandleProperty(ColorProperty property, ExcelReportCell cell)
        {
            cell.FontColor = property.FontColor;
            cell.BackgroundColor = property.BackgroundColor;
        }
    }
}
