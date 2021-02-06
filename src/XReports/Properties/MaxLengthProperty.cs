using System;
using XReports.Models;

namespace XReports.Properties
{
    public class MaxLengthProperty : ReportCellProperty
    {
        public MaxLengthProperty(int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative");
            }

            this.MaxLength = maxLength;
        }

        public int MaxLength { get; }
    }
}
