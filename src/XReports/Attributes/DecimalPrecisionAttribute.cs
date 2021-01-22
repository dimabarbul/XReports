using System;

namespace XReports.Attributes
{
    public class DecimalPrecisionAttribute : AttributeBase
    {
        public int Precision { get; }

        public DecimalPrecisionAttribute(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }
    }
}
