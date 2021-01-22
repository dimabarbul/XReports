using System;

namespace XReports.Attributes
{
    public class DateTimeFormatAttribute : AttributeBase
    {
        public string Format { get; }

        public DateTimeFormatAttribute(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format cannot be empty or null", nameof(format));
            }

            this.Format = format;
        }
    }
}
