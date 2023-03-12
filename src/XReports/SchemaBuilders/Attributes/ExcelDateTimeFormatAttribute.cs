using System;

namespace XReports.SchemaBuilders.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class ExcelDateTimeFormatAttribute : Attribute
    {
        public ExcelDateTimeFormatAttribute(string format, string excelFormat)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format cannot be empty or null", nameof(format));
            }

            if (string.IsNullOrEmpty(excelFormat))
            {
                throw new ArgumentException("Excel format cannot be empty or null", nameof(excelFormat));
            }

            this.Format = format;

            this.ExcelFormat = excelFormat;
        }

        public string Format { get; }

        public string ExcelFormat { get; }
    }
}
