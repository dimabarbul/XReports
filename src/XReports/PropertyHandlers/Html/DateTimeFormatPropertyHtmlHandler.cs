using System;
using System.Globalization;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class DateTimeFormatPropertyHtmlHandler : PropertyHandler<DateTimeFormatProperty, HtmlReportCell>
    {
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
