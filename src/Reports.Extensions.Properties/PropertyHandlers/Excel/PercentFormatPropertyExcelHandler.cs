using System.Linq;
using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class PercentFormatPropertyExcelHandler : PropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))} %";
        }
    }
}
