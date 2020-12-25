using Reports.Models;

namespace Reports.Properties
{
    public class MaxLengthProperty : ReportCellProperty
    {
        public int MaxLength { get; }

        public MaxLengthProperty(int maxLength)
        {
            this.MaxLength = maxLength;
        }
    }
}
