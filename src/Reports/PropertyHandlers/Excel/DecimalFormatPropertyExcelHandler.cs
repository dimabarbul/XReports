using System.Linq;
using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Excel
{
    public class DecimalFormatPropertyExcelHandler : PropertyHandler<DecimalFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}";
        }
    }
}
