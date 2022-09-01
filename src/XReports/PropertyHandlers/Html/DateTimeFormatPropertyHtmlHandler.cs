using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class DateTimeFormatPropertyHtmlHandler : PropertyHandler<DateTimeFormatProperty, HtmlReportCell>
    {
        private const string FormatDayWithLeadingZero = "dd";
        private const string FormatDay = "d";
        private const string FormatMonthWithLeadingZero = "MM";
        private const string FormatMonth = "M";
        private const string FormatYearFull = "yyyy";
        private const string FormatYearShort = "yy";
        private const string FormatMonthName = "MMMM";
        private const string FormatHourWithLeadingZero = "HH";
        private const string FormatHour = "H";
        private const string FormatHour12WithLeadingZero = "hh";
        private const string FormatHour12 = "h";
        private const string FormatMinuteWithLeadingZero = "mm";
        private const string FormatMinute = "m";
        private const string FormatSecondWithLeadingZero = "ss";
        private const string FormatSecond = "s";
        private const string FormatAmPm = "tt";

        private readonly Dictionary<DateTimeFormatProperty, string> formatCache = new Dictionary<DateTimeFormatProperty, string>();

        protected override void HandleProperty(DateTimeFormatProperty property, HtmlReportCell cell)
        {
            cell.SetValue(cell.GetNullableValue<DateTime>()?.ToString(this.GetFormatString(property), CultureInfo.CurrentCulture));
        }

        private string GetFormatString(DateTimeFormatProperty property)
        {
            if (!this.formatCache.ContainsKey(property))
            {
                this.formatCache[property] = string.Concat(property.Parts.Select(this.GetFormatPartString));
            }

            return this.formatCache[property];
        }

        private string GetFormatPartString(DateTimeFormatPart formatPart)
        {
            return formatPart.Type switch
            {
                DateTimeFormatPartType.FreeText => formatPart.Text,
                DateTimeFormatPartType.DayWithLeadingZero => FormatDayWithLeadingZero,
                DateTimeFormatPartType.Day => FormatDay,
                DateTimeFormatPartType.MonthWithLeadingZero => FormatMonthWithLeadingZero,
                DateTimeFormatPartType.Month => FormatMonth,
                DateTimeFormatPartType.YearFull => FormatYearFull,
                DateTimeFormatPartType.YearShort => FormatYearShort,
                DateTimeFormatPartType.MonthName => FormatMonthName,
                DateTimeFormatPartType.HourWithLeadingZero => FormatHourWithLeadingZero,
                DateTimeFormatPartType.Hour => FormatHour,
                DateTimeFormatPartType.Hour12WithLeadingZero => FormatHour12WithLeadingZero,
                DateTimeFormatPartType.Hour12 => FormatHour12,
                DateTimeFormatPartType.MinuteWithLeadingZero => FormatMinuteWithLeadingZero,
                DateTimeFormatPartType.Minute => FormatMinute,
                DateTimeFormatPartType.SecondWithLeadingZero => FormatSecondWithLeadingZero,
                DateTimeFormatPartType.Second => FormatSecond,
                DateTimeFormatPartType.AmPm => FormatAmPm,
                _ => throw new ArgumentOutOfRangeException(nameof(formatPart)),
            };
        }
    }
}
