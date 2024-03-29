using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="PercentFormatProperty"/> during conversion to HTML.
    /// </summary>
    public class PercentFormatPropertyHtmlHandler : PropertyHandler<PercentFormatProperty, HtmlReportCell>
    {
        private readonly Dictionary<(bool, int), string> formatCache = new Dictionary<(bool, int), string>();

        /// <inheritdoc />
        protected override void HandleProperty(PercentFormatProperty property, HtmlReportCell cell)
        {
            decimal? value = cell.GetNullableValue<decimal>();
            if (value == null)
            {
                return;
            }

            string format = this.GetFormat(property);
            cell.SetValue((value.Value * 100).ToString(format, CultureInfo.CurrentCulture) + property.PostfixText);
        }

        private string GetFormat(PercentFormatProperty property)
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
