using System;
using XReports.Models;

namespace XReports.Properties
{
    public class PercentFormatProperty : ReportCellProperty
    {
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

        public int Precision { get; }

        public string PostfixText { get; }

        public bool PreserveTrailingZeros { get; }
    }
}
