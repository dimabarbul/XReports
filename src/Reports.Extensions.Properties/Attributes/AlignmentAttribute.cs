using Reports.Core.Enums;
using Reports.Extensions.AttributeBasedBuilder.Attributes;

namespace Reports.Extensions.Properties.Attributes
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
