using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="DecimalPrecisionProperty"/> during conversion to HTML.
    /// </summary>
    public class DecimalPrecisionPropertyHtmlHandler : PropertyHandler<DecimalPrecisionProperty, HtmlReportCell>
    {
        private readonly Dictionary<(bool, int), string> formatCache = new Dictionary<(bool, int), string>();

        /// <inheritdoc />
        protected override void HandleProperty(DecimalPrecisionProperty property, HtmlReportCell cell)
        {
            string format = this.GetFormat(property);
            cell.SetValue(cell.GetNullableValue<decimal>()?.ToString(format, CultureInfo.CurrentCulture));
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
