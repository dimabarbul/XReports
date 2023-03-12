using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    public class BoldPropertyExcelHandler : PropertyHandler<BoldProperty, ExcelReportCell>
    {
        protected override void HandleProperty(BoldProperty property, ExcelReportCell cell)
        {
            cell.IsBold = true;
        }
    }
}
