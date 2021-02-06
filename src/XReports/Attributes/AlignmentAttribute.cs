using System;
using System.ComponentModel;
using XReports.Enums;

namespace XReports.Attributes
{
    public class AlignmentAttribute : AttributeBase
    {
        public AlignmentAttribute(AlignmentType alignment)
        {
            if (!Enum.IsDefined(typeof(AlignmentType), alignment))
            {
                throw new InvalidEnumArgumentException(nameof(alignment), (int)alignment, typeof(AlignmentType));
            }

            this.Alignment = alignment;
        }

        public AlignmentType Alignment { get; }
    }
}
