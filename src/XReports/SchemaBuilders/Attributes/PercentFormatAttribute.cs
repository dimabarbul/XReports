using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies percent format for cells with numeric value. Usually this means that, for example, value 0.1 will be displayed as "10%" or alike.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class PercentFormatAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PercentFormatAttribute"/> class.
        /// </summary>
        /// <param name="precision">Count of decimal places to display.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="precision"/> is less than 0.</exception>
        public PercentFormatAttribute(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }

        /// <summary>
        /// Gets count of decimal places to display.
        /// </summary>
        public int Precision { get; }

        /// <summary>
        /// Gets or sets a value indicating whether value indicating whether trailing zeros should be displayed.
        /// </summary>
        public bool PreserveTrailingZeros { get; set; } = true;

        /// <summary>
        /// Gets or sets text to append to value.
        /// </summary>
        public string PostfixText { get; set; } = "%";
    }
}
