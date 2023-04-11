using System;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells that should have different format in Excel and in other report types.
    /// </summary>
    public class ExcelDateTimeFormatProperty : DateTimeFormatProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelDateTimeFormatProperty"/> class.
        /// </summary>
        /// <param name="format">Format to apply in non-Excel report types.</param>
        /// <param name="excelFormat">Format to apply in Excel report type.</param>
        /// <exception cref="ArgumentNullException">Thrown when Excel format is null or empty.</exception>
        public ExcelDateTimeFormatProperty(string format, string excelFormat)
            : base(format)
        {
            if (string.IsNullOrEmpty(excelFormat))
            {
                throw new ArgumentNullException(nameof(excelFormat));
            }

            this.ExcelFormat = excelFormat;
        }

        /// <summary>
        /// Gets format to apply in Excel.
        /// </summary>
        public string ExcelFormat { get; }
    }
}
