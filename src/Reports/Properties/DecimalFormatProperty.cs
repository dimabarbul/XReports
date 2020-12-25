using Reports.Models;

namespace Reports.Properties
{
    public class DecimalFormatProperty : ReportCellProperty
    {
        public int Precision { get; }

        public DecimalFormatProperty(int precision)
        {
            this.Precision = precision;
        }
    }
}
