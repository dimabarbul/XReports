using Reports.Core.Models;

namespace Reports.Extensions.Properties
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
