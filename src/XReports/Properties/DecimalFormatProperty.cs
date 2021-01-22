using System;
using XReports.Models;

namespace XReports.Properties
{
    public class DecimalFormatProperty : ReportCellProperty
    {
        public int Precision { get; }

        public DecimalFormatProperty(int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");
            }

            this.Precision = precision;
        }
    }
}
