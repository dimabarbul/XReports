using System;
using XReports.Helpers;
using XReports.Models;

namespace XReports.Properties
{
    public class DateTimeFormatProperty : ReportCellProperty
    {
        public DateTimeFormatProperty(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format cannot be empty or null", nameof(format));
            }

            this.Parts = DateTimeParser.Parse(format);
        }

        public DateTimeFormatPart[] Parts { get; }
    }
}
