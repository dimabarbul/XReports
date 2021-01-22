using System;

namespace XReports.Attributes
{
    public class DecimalFormatAttribute : AttributeBase
    {
        public int Precision { get; }

        public DecimalFormatAttribute(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }
    }
}
