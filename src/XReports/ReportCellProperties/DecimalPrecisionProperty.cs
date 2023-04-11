using System;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    /// <summary>
    /// Property to mark cells which value is numeric and should be displayed with fixed precision.
    /// </summary>
    public class DecimalPrecisionProperty : ReportCellProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalPrecisionProperty"/> class.
        /// </summary>
        /// <param name="precision">Count of decimal places to display.</param>
        /// <param name="preserveTrailingZeros">Value indicating whether trailing zeros should be displayed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if precision is less that 0.</exception>
        public DecimalPrecisionProperty(int precision, bool preserveTrailingZeros = true)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
            this.PreserveTrailingZeros = preserveTrailingZeros;
        }

        /// <summary>
        /// Gets count of decimal places to display.
        /// </summary>
        public int Precision { get; }

        /// <summary>
        /// Gets a value indicating whether trailing zeros should be displayed.
        /// </summary>
        public bool PreserveTrailingZeros { get; }
    }
}
