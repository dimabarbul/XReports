using System;
using XReports.Converter;
using XReports.ReportCellProperties;

namespace XReports.Excel.PropertyHandlers
{
    /// <summary>
    /// Handler for <see cref="DateTimeFormatProperty"/> during conversion to Excel.
    /// </summary>
    public class DateTimeFormatPropertyExcelHandler : PropertyHandler<DateTimeFormatProperty, ExcelReportCell>
    {
        /// <inheritdoc />
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
