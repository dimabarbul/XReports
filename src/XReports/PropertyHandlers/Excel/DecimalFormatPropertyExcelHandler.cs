using System.Linq;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class DecimalFormatPropertyExcelHandler : PropertyHandler<DecimalFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}";
        }
    }
}
