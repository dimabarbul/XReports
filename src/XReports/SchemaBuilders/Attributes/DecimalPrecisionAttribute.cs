using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies precision when displaying values of cells with numeric value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalPrecisionAttribute"/> class.
        /// </summary>
        /// <param name="precision">Count of decimal places to display.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if precision is less that 0.</exception>
        public DecimalPrecisionAttribute(int precision)
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
        /// Gets or sets a value indicating whether trailing zeros should be displayed.
        /// </summary>
        public bool PreserveTrailingZeros { get; set; } = true;
    }
}
