using System.Linq;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class PercentFormatPropertyExcelHandler : PropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            if (!(property.PostfixText ?? string.Empty).Contains('%'))
            {
                cell.InternalValue = cell.GetNullableValue<decimal>() * 100;
            }

            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}{property.PostfixText}";
        }
    }
}
