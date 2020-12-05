using System.Linq;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class DecimalFormatPropertyExcelHandler : PropertyHandler<DecimalFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}";
        }
    }
}
