using Reports.Enums;

namespace Reports.Models.Properties
{
    public class AlignmentProperty
    {
        private readonly AlignmentType alignmentType;

        public AlignmentProperty(AlignmentType alignmentType)
        {
            this.alignmentType = alignmentType;
        }
    }
}
