using Reports.Excel.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.Handlers.Excel
{
    public class ExcelBoldPropertyHandler : SingleTypePropertyHandler<BoldProperty, ExcelReportCell>
    {
        protected override void HandleProperty(BoldProperty property, ExcelReportCell cell)
        {
            cell.IsBold = true;
        }
    }
}
