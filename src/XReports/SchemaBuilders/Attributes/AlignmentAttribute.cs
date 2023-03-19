using System;
using System.ComponentModel;
using XReports.ReportCellProperties;

namespace XReports.SchemaBuilders.Attributes
{
    public sealed class AlignmentAttribute : BasePropertyAttribute
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
