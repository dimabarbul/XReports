using System;
using XReports.Table;

namespace XReports.Properties
{
    public class DateTimeFormatProperty : ReportCellProperty
    {
        public DateTimeFormatProperty(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            this.Format = format;
        }

        public string Format { get; }
    }
}
