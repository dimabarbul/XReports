using XReports.Enums;
using XReports.Models;

namespace XReports.Properties
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
