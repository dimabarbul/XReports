using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class AlignmentPropertyExcelHandler : PropertyHandler<AlignmentProperty, ExcelReportCell>
    {
        protected override void HandleProperty(AlignmentProperty property, ExcelReportCell cell)
        {
            cell.HorizontalAlignment = property.Alignment;
        }
    }
}
