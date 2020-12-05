using Reports.Core.Enums;
using Reports.Core.Models;

namespace Reports.Extensions.Properties
{
    public class AlignmentProperty : ReportCellProperty
    {
        public AlignmentType AlignmentType { get; }

        public AlignmentProperty(AlignmentType alignmentType)
        {
            this.AlignmentType = alignmentType;
        }
    }
}
