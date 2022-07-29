using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class DecimalPrecisionPropertyHtmlHandler : PropertyHandler<DecimalPrecisionProperty, HtmlReportCell>
    {
        private readonly Dictionary<int, string> formatCache = new Dictionary<int, string>();

        protected override void HandleProperty(DecimalPrecisionProperty property, HtmlReportCell cell)
        {
            string format = this.GetFormat(property.Precision);
            cell.SetValue(cell.GetNullableValue<decimal>()?.ToString(format));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetFormat(int precision)
        {
            if (!this.formatCache.ContainsKey(precision))
            {
                this.formatCache[precision] = $"F{precision}";
            }

            return this.formatCache[precision];
        }
    }
}
