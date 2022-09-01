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
            switch (formatPart.Type)
            {
                case DateTimeFormatPartType.FreeText:
                    return formatPart.Text;
                case DateTimeFormatPartType.DayWithLeadingZero:
                    return FormatDayWithLeadingZero;
                case DateTimeFormatPartType.Day:
                    return FormatDay;
                case DateTimeFormatPartType.MonthWithLeadingZero:
                    return FormatMonthWithLeadingZero;
                case DateTimeFormatPartType.Month:
                    return FormatMonth;
                case DateTimeFormatPartType.YearFull:
                    return FormatYearFull;
                case DateTimeFormatPartType.YearShort:
                    return FormatYearShort;
                case DateTimeFormatPartType.MonthName:
                    return FormatMonthName;
                case DateTimeFormatPartType.HourWithLeadingZero:
                    return FormatHourWithLeadingZero;
                case DateTimeFormatPartType.Hour:
                    return FormatHour;
                case DateTimeFormatPartType.Hour12WithLeadingZero:
                    return FormatHour12WithLeadingZero;
                case DateTimeFormatPartType.Hour12:
                    return FormatHour12;
                case DateTimeFormatPartType.MinuteWithLeadingZero:
                    return FormatMinuteWithLeadingZero;
                case DateTimeFormatPartType.Minute:
                    return FormatMinute;
                case DateTimeFormatPartType.SecondWithLeadingZero:
                    return FormatSecondWithLeadingZero;
                case DateTimeFormatPartType.Second:
                    return FormatSecond;
                case DateTimeFormatPartType.AmPm:
                    return FormatAmPm;
                default:
                    throw new ArgumentOutOfRangeException(nameof(formatPart));
            }
        }
    }
}
