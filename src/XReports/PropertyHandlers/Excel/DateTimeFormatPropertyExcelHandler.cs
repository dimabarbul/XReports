using System;
using XReports.Converter;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class DateTimeFormatPropertyExcelHandler : PropertyHandler<DateTimeFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DateTimeFormatProperty property, ExcelReportCell cell)
        {
            object value = cell.GetUnderlyingValue();
            if (value is DateTimeOffset dateTimeOffset)
            {
                cell.SetValue(dateTimeOffset.DateTime);
            }
            else if (value != null)
            {
                // Ensure the value is convertible to DateTime.
                _ = cell.GetValue<DateTime>();
            }

            cell.NumberFormat = property.Format;
        }
    }
}
