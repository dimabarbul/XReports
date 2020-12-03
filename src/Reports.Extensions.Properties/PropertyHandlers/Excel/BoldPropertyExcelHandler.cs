using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class BoldPropertyExcelHandler : PropertyHandler<BoldProperty, ExcelReportCell>
    {
        protected override void HandleProperty(BoldProperty property, ExcelReportCell cell)
        {
            cell.IsBold = true;
        }
    }
}
