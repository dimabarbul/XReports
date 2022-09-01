using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class DateTimeFormatPropertyExcelHandler : PropertyHandler<DateTimeFormatProperty, ExcelReportCell>
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
        private const string FormatAmPm = "AM/PM";

        private readonly Dictionary<DateTimeFormatProperty, string> formatCache = new Dictionary<DateTimeFormatProperty, string>();

        protected override void HandleProperty(DateTimeFormatProperty property, ExcelReportCell cell)
        {
            this.ValidateFormat(property.Parts);

            cell.NumberFormat = this.GetFormatString(property);
        }

        private void ValidateFormat(IReadOnlyList<DateTimeFormatPart> propertyParts)
        {
            bool has12HourPart = this.Has12HourPart(propertyParts);
            bool has24HourPart = this.Has24HourPart(propertyParts);
            bool hasAmPmPart = this.HasAmPmPart(propertyParts);

            if (has12HourPart && !hasAmPmPart)
            {
                throw new InvalidOperationException("To get hour in 12-hour format, please, add am/pm part");
            }

            if (has24HourPart && hasAmPmPart)
            {
                throw new InvalidOperationException("Cannot show am/pm part when hour in 24-hour format is added");
            }
        }

        private bool HasAmPmPart(IReadOnlyList<DateTimeFormatPart> propertyParts)
        {
            for (int i = 0; i < propertyParts.Count; i++)
            {
                if (propertyParts[i].Type == DateTimeFormatPartType.AmPm)
                {
                    return true;
                }
            }

            return false;
        }

        private bool Has24HourPart(IReadOnlyList<DateTimeFormatPart> propertyParts)
        {
            for (int i = 0; i < propertyParts.Count; i++)
            {
                if (propertyParts[i].Type == DateTimeFormatPartType.Hour
                    || propertyParts[i].Type == DateTimeFormatPartType.HourWithLeadingZero)
                {
                    return true;
                }
            }

            return false;
        }

        private bool Has12HourPart(IReadOnlyList<DateTimeFormatPart> propertyParts)
        {
            for (int i = 0; i < propertyParts.Count; i++)
            {
                if (propertyParts[i].Type == DateTimeFormatPartType.Hour12
                    || propertyParts[i].Type == DateTimeFormatPartType.Hour12WithLeadingZero)
                {
                    return true;
                }
            }

            return false;
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
