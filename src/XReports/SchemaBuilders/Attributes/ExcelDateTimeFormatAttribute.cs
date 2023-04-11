using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies display format for cells with DateTime value which should have different format in Excel and in other report types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class ExcelDateTimeFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelDateTimeFormatAttribute"/> class.
        /// </summary>
        /// <param name="format">Format to apply in non-Excel report types.</param>
        /// <param name="excelFormat">Format to apply in Excel report type.</param>
        /// <exception cref="ArgumentNullException">Thrown when Excel format is null or empty.</exception>
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

        /// <summary>
        /// Gets format to apply.
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// Gets format to apply in Excel.
        /// </summary>
        public string ExcelFormat { get; }
    }
}
