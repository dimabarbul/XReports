using System;
using Reports.Interfaces;

namespace Reports.ValueFormatters
{
    public class DateTimeValueFormatter : IValueFormatter<DateTime>
    {
        private readonly string format;

        public DateTimeValueFormatter(string format)
        {
            this.format = format;
        }

        public string Format(DateTime value)
        {
            return value.ToString(this.format);
        }
    }
}
