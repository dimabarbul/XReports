using System;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class PercentFormatAttribute : Attribute
    {
        public PercentFormatAttribute(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }

        public int Precision { get; }

        public bool PreserveTrailingZeros { get; set; } = true;

        public string PostfixText { get; set; } = "%";
    }
}
