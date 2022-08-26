using System;
using System.ComponentModel;
using XReports.Enums;

namespace XReports.Attributes
{
    public class AlignmentAttribute : BasePropertyAttribute
    {
        public AlignmentAttribute(Alignment alignment)
        {
            if (!Enum.IsDefined(typeof(Alignment), alignment))
            {
                throw new InvalidEnumArgumentException(nameof(alignment), (int)alignment, typeof(Alignment));
            }

            this.Alignment = alignment;
        }

        public Alignment Alignment { get; }
    }
}
