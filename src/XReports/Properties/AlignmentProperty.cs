using System;
using System.ComponentModel;
using XReports.Enums;
using XReports.Models;

namespace XReports.Properties
{
    public class AlignmentProperty : ReportCellProperty
    {
        public AlignmentProperty(AlignmentType alignmentType)
        {
            if (!Enum.IsDefined(typeof(AlignmentType), alignmentType))
            {
                throw new InvalidEnumArgumentException(nameof(alignmentType), (int)alignmentType, typeof(AlignmentType));
            }

            this.AlignmentType = alignmentType;
        }

        public AlignmentType AlignmentType { get; }
    }
}
