using Reports.Interfaces;

namespace Reports.Extensions.Properties
{
    public class PercentFormatProperty : IReportCellProperty
    {
        public int Precision { get; }

        public PercentFormatProperty(int precision)
        {
            this.Precision = precision;
        }
    }
}
