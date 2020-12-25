using XReports.Models;

namespace XReports.Properties
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
