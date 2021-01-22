using System.Linq;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class DecimalPrecisionPropertyExcelHandler : PropertyHandler<DecimalPrecisionProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DecimalPrecisionProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}";
        }
    }
}
