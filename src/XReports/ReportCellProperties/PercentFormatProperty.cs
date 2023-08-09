using System;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells which contain numeric value and it should be displayed as percent. Usually this means that, for example, value 0.1 will be displayed as "10%" or alike.
    /// </summary>
    public class PercentFormatProperty : IReportCellProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PercentFormatProperty"/> class.
        /// </summary>
        /// <param name="precision">Count of decimal places to display.</param>
        /// <param name="postfixText">Text to append to value.</param>
        /// <param name="preserveTrailingZeros">Value indicating whether trailing zeros should be displayed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="precision"/> is less than 0.</exception>
        public PercentFormatProperty(int precision, string postfixText = "%", bool preserveTrailingZeros = true)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
            this.PostfixText = postfixText;
            this.PreserveTrailingZeros = preserveTrailingZeros;
        }

        /// <summary>
        /// Gets count of decimal places to display.
        /// </summary>
        public int Precision { get; }

        /// <summary>
        /// Gets text to append to value.
        /// </summary>
        public string PostfixText { get; }

        /// <summary>
        /// Gets a value indicating whether value indicating whether trailing zeros should be displayed.
        /// </summary>
        public bool PreserveTrailingZeros { get; }
    }
}
