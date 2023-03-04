using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class DecimalPrecisionPropertyExcelHandler : PropertyHandler<DecimalPrecisionProperty, ExcelReportCell>
    {
        private readonly Dictionary<(bool, int), string> formatCache = new Dictionary<(bool, int), string>();

        protected override void HandleProperty(DecimalPrecisionProperty property, ExcelReportCell cell)
        {
            // Ensure the value is convertible to decimal.
            _ = cell.GetNullableValue<decimal>();

            string format = this.GetFormat(property);
            cell.NumberFormat = format;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetFormat(DecimalPrecisionProperty property)
        {
            (bool, int) key = (property.PreserveTrailingZeros, property.Precision);
            if (!this.formatCache.ContainsKey(key))
            {
                this.formatCache[key] = $"0.{string.Concat(Enumerable.Repeat(property.PreserveTrailingZeros ? '0' : '#', property.Precision))}";
            }

            return this.formatCache[key];
        }
    }
}
