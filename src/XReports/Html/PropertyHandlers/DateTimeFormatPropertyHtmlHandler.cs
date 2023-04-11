using System;
using System.Globalization;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Html.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="DateTimeFormatProperty"/> during conversion to HTML.
    /// </summary>
    public class DateTimeFormatPropertyHtmlHandler : PropertyHandler<DateTimeFormatProperty, HtmlReportCell>
    {
        /// <inheritdoc />
        protected override void HandleProperty(DateTimeFormatProperty property, HtmlReportCell cell)
        {
            object value = cell.GetUnderlyingValue();
            if (value is null)
            {
                return;
            }

            if (value is DateTimeOffset dateTimeOffset)
            {
                cell.SetValue(dateTimeOffset.ToString(property.Format, CultureInfo.CurrentCulture));
                return;
            }

            DateTime dateTime = cell.GetValue<DateTime>();

            cell.SetValue(dateTime.ToString(property.Format, CultureInfo.CurrentCulture));
        }
    }
}
