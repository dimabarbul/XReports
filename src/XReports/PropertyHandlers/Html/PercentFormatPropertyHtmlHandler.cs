using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class PercentFormatPropertyHtmlHandler : PropertyHandler<PercentFormatProperty, HtmlReportCell>
    {
        private readonly Dictionary<int, string> formatCache = new Dictionary<int, string>();

        public override int Priority => (int)HtmlPropertyHandlerPriority.Text;

        protected override void HandleProperty(PercentFormatProperty property, HtmlReportCell cell)
        {
            string format = this.GetFormat(property.Precision);
            cell.SetValue((cell.GetNullableValue<decimal>() * 100)?.ToString(format) + property.PostfixText);
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
