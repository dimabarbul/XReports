using System;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells with DateTime value that should be displayed using certain format.
    /// </summary>
    public class DateTimeFormatProperty : ReportCellProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFormatProperty"/> class.
        /// </summary>
        /// <param name="format">Format to apply.</param>
        /// <exception cref="ArgumentNullException">Thrown when format is null or empty.</exception>
        public DateTimeFormatProperty(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            this.Format = format;
        }

        /// <summary>
        /// Gets format to apply.
        /// </summary>
        public string Format { get; }
    }
}
