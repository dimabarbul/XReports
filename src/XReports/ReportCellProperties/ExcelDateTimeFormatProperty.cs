using System;

namespace XReports.ReportCellProperties
{
    public class ExcelDateTimeFormatProperty : DateTimeFormatProperty
    {
        public ExcelDateTimeFormatProperty(string format, string excelFormat)
            : base(format)
        {
            if (string.IsNullOrEmpty(excelFormat))
            {
                throw new ArgumentNullException(nameof(excelFormat));
            }

            this.ExcelFormat = excelFormat;
        }

        public string ExcelFormat { get; }
    }
}