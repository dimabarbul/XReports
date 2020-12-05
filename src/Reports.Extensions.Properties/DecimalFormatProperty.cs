using Reports.Models;

namespace Reports.Extensions.Properties
{
    public class DecimalFormatProperty : ReportCellProperty
    {
        public int Precision { get; }
        public bool CultureAgnostic { get; set; }

        public DecimalFormatProperty(int precision)
        {
            this.Precision = precision;
        }
    }
}
