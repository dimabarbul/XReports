using System;

namespace XReports.Attributes
{
    public class PercentFormatAttribute : AttributeBase
    {
        public int Precision { get; }
        public string PostfixText { get; set; }

        public PercentFormatAttribute(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }
    }
}
