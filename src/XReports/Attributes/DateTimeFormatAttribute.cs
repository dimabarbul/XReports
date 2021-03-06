using System;

namespace XReports.Attributes
{
    public class DateTimeFormatAttribute : AttributeBase
    {
        public DateTimeFormatAttribute(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format cannot be empty or null", nameof(format));
            }

            this.Format = format;
        }

        public string Format { get; }
    }
}
