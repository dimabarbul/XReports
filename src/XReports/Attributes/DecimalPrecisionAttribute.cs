using System;

namespace XReports.Attributes
{
    public sealed class DecimalPrecisionAttribute : BasePropertyAttribute
    {
        public DecimalPrecisionAttribute(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }

        public int Precision { get; }

        public bool PreserveTrailingZeros { get; set; } = true;
    }
}
