using System;
using System.Linq;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class DateTimeFormatPropertyHtmlHandler : PropertyHandler<DateTimeFormatProperty, HtmlReportCell>
    {
        protected override void HandleProperty(DateTimeFormatProperty property, HtmlReportCell cell)
        {
            cell.Value = cell.GetNullableValue<DateTime>()?.ToString(this.GetFormatString(property));
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
                DateTimeFormatPartType.AmPm => "tt",
                _ => throw new ArgumentOutOfRangeException(nameof(formatPart)),
            };
        }
    }
}
