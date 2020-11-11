using Reports.Enums;
using Reports.Interfaces;

namespace Reports.Extensions.Properties
{
    public class AlignmentProperty : IReportCellProperty
    {
        public AlignmentType AlignmentType { get; }

        public AlignmentProperty(AlignmentType alignmentType)
        {
            this.AlignmentType = alignmentType;
        }
    }
}
