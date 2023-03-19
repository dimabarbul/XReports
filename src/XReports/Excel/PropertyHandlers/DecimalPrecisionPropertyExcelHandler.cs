using System.Collections.Generic;
using System.Linq;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
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
