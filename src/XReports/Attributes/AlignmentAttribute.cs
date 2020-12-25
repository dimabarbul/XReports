using XReports.Enums;

namespace XReports.Attributes
{
    public class AlignmentAttribute : AttributeBase
    {
        public AlignmentType Alignment { get; }

        public AlignmentAttribute(AlignmentType alignment)
        {
            this.Alignment = alignment;
        }
    }
}
