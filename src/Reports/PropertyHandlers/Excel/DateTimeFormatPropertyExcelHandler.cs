using System;
using System.Linq;
using Reports.Enums;
using Reports.Models;
using Reports.Properties;

namespace Reports.PropertyHandlers.Excel
{
    public class DateTimeFormatPropertyExcelHandler : PropertyHandler<DateTimeFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(DateTimeFormatProperty property, ExcelReportCell cell)
        {
            this.ValidateFormat(property.Parts);

            cell.NumberFormat = this.GetFormatString(property);
        }

        private void ValidateFormat(DateTimeFormatPart[] propertyParts)
        {
            bool has12HourPart = propertyParts.Any(
                p => p.Type == DateTimeFormatPartType.Hour12
                    || p.Type == DateTimeFormatPartType.Hour12WithLeadingZero);
            bool has24HourPart = propertyParts.Any(
                p => p.Type == DateTimeFormatPartType.Hour
                    || p.Type == DateTimeFormatPartType.HourWithLeadingZero);
            bool hasAmPmPart = propertyParts.Any(
                p => p.Type == DateTimeFormatPartType.AmPm);

            if (has12HourPart && !hasAmPmPart)
            {
                throw new InvalidOperationException("To get hour in 12-hour format, please, add am/pm part");
            }

            if (has24HourPart && hasAmPmPart)
            {
                throw new InvalidOperationException("Cannot show am/pm part when hour in 24-hour format is added");
            }
        }

        private string GetFormatString(DateTimeFormatProperty property)
        {
            return string.Concat(property.Parts.Select(this.GetFormatPartString));
        }

        private string GetFormatPartString(DateTimeFormatPart formatPart)
        {
            return formatPart.Type switch
            {
                DateTimeFormatPartType.FreeText => formatPart.Text,
                DateTimeFormatPartType.DayWithLeadingZero => "dd",
                DateTimeFormatPartType.Day => "d",
                DateTimeFormatPartType.MonthWithLeadingZero => "MM",
                DateTimeFormatPartType.Month => "M",
                DateTimeFormatPartType.YearFull => "yyyy",
                DateTimeFormatPartType.YearShort => "yy",
                DateTimeFormatPartType.MonthName => "MMMM",
                DateTimeFormatPartType.HourWithLeadingZero => "HH",
                DateTimeFormatPartType.Hour => "H",
                DateTimeFormatPartType.Hour12WithLeadingZero => "hh",
                DateTimeFormatPartType.Hour12 => "h",
                DateTimeFormatPartType.MinuteWithLeadingZero => "mm",
                DateTimeFormatPartType.Minute => "m",
                DateTimeFormatPartType.SecondWithLeadingZero => "ss",
                DateTimeFormatPartType.Second => "s",
                DateTimeFormatPartType.AmPm => "AM/PM",
                _ => throw new ArgumentOutOfRangeException(nameof(formatPart)),
            };
        }
    }
}
