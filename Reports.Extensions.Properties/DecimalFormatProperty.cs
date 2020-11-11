using Reports.Interfaces;

namespace Reports.Extensions.Properties
{
    public class DecimalFormatProperty : IReportCellProperty
    {
        public int Precision { get; private set; }

        public DecimalFormatProperty(int precision)
        {
            this.Precision = precision;
        }
    }
}
