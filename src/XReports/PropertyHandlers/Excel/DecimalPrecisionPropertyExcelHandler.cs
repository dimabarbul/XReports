using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class DecimalPrecisionPropertyExcelHandler : PropertyHandler<DecimalPrecisionProperty, ExcelReportCell>
    {
        private readonly Dictionary<int, string> formatCache = new Dictionary<int, string>();

        protected override void HandleProperty(DecimalPrecisionProperty property, ExcelReportCell cell)
        {
            string format = this.GetFormat(property.Precision);
            cell.NumberFormat = format;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetFormat(int precision)
        {
            if (!this.formatCache.ContainsKey(precision))
            {
                this.formatCache[precision] = $"0.{string.Concat(Enumerable.Repeat('0', precision))}";
            }

            return this.formatCache[precision];
        }
    }
}
