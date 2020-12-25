using Reports.Enums;

namespace Reports.Attributes
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
