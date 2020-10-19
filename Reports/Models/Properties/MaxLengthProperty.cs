using Reports.Interfaces;

namespace Reports.Models.Properties
{
    public class MaxLengthProperty : IReportCellProperty
    {
        public int MaxLength { get; }

        public MaxLengthProperty(int maxLength)
        {
            this.MaxLength = maxLength;
        }
    }
}
