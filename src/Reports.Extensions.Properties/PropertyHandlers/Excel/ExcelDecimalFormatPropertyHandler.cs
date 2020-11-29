using System.Linq;
using Reports.Excel.Models;
using Reports.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class ExcelDecimalFormatPropertyHandler : SingleTypePropertyHandler<DecimalFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DecimalFormatProperty property, ExcelReportCell cell)
        {
            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}";
        }
    }
}
