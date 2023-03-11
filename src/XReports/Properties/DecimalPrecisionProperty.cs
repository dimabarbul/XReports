using System;
using XReports.Table;

namespace XReports.Properties
{
    public class DecimalPrecisionProperty : ReportCellProperty
    {
        public DecimalPrecisionProperty(int precision, bool preserveTrailingZeros = true)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
            this.PreserveTrailingZeros = preserveTrailingZeros;
        }

        public int Precision { get; }

        public bool PreserveTrailingZeros { get; }
    }
}
