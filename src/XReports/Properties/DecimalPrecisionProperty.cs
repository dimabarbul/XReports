using System;
using XReports.Models;

namespace XReports.Properties
{
    public class DecimalPrecisionProperty : ReportCellProperty
    {
        public DecimalPrecisionProperty(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }

        public int Precision { get; }
    }
}
