using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Extensions.Properties.Enums;
using Reports.Extensions.Properties.Models;

namespace Reports.Extensions.Properties.Helpers
{
    public static class DateTimeParser
    {
        private static readonly Dictionary<string, DateTimeFormatPartType> TokenToPartTypeMap = new Dictionary<string, DateTimeFormatPartType>()
        {
            { "dd", DateTimeFormatPartType.DayWithLeadingZero },
            { "d", DateTimeFormatPartType.Day },
            { "MM", DateTimeFormatPartType.MonthWithLeadingZero },
            { "M", DateTimeFormatPartType.Month },
            { "yyyy", DateTimeFormatPartType.YearFull },
            { "yy", DateTimeFormatPartType.YearShort },
            { "MMMM", DateTimeFormatPartType.MonthName },
            { "HH", DateTimeFormatPartType.HourWithLeadingZero },
            { "H", DateTimeFormatPartType.Hour },
            { "hh", DateTimeFormatPartType.Hour12WithLeadingZero },
            { "h", DateTimeFormatPartType.Hour12 },
            { "mm", DateTimeFormatPartType.MinuteWithLeadingZero },
            { "m", DateTimeFormatPartType.Minute },
            { "ss", DateTimeFormatPartType.SecondWithLeadingZero },
            { "s", DateTimeFormatPartType.Second },
            { "tt", DateTimeFormatPartType.AmPm },
        };

        public static DateTimeFormatPart[] Parse(string format)
        {
            List<DateTimeFormatPart> parts = new List<DateTimeFormatPart>();
            string[] orderedTokens = TokenToPartTypeMap.Keys
                .OrderByDescending(t => t.Length)
                .ToArray();
            string lastFreeTextToken = null;
            string token;
            string toParse = format;

            while (!string.IsNullOrWhiteSpace(toParse))
            {
                token = orderedTokens.FirstOrDefault(t => toParse.StartsWith(t, StringComparison.Ordinal));
                if (token == null)
                {
                    token = toParse.Substring(0, 1);
                    lastFreeTextToken = (lastFreeTextToken ?? string.Empty) + token;
                }
                else
                {
                    if (lastFreeTextToken != null)
                    {
                        parts.Add(new DateTimeFormatPart(lastFreeTextToken));
                        lastFreeTextToken = null;
                    }

                    parts.Add(new DateTimeFormatPart(TokenToPartTypeMap[token]));
                }

                toParse = toParse.Remove(0, token.Length);
            }

            return parts.ToArray();
        }
    }
}
