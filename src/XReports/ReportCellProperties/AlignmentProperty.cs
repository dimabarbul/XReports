using System;
using System.ComponentModel;
using XReports.Table;

namespace XReports.ReportCellProperties
{
    public class AlignmentProperty : ReportCellProperty
    {
        public AlignmentProperty(Alignment alignment)
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
