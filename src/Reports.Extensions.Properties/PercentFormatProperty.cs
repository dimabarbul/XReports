using Reports.Core.Models;

namespace Reports.Extensions.Properties
{
    public class PercentFormatProperty : ReportCellProperty
    {
        public int Precision { get; }

        public PercentFormatProperty(int precision)
        {
            this.Precision = precision;
        }
    }
}
