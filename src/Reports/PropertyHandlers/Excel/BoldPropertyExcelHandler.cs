using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Excel
{
    public class BoldPropertyExcelHandler : PropertyHandler<BoldProperty, ExcelReportCell>
    {
        protected override void HandleProperty(BoldProperty property, ExcelReportCell cell)
        {
            cell.IsBold = true;
        }
    }
}
