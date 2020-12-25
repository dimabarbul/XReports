using System;
using Reports.Enums;
using Reports.Extensions;
using Reports.Interfaces;

namespace Reports.Attributes
{
    public class VerticalReportAttribute : ReportAttribute
    {
        public override Type PostBuilder
        {
            get => base.PostBuilder;
            set
            {
                if (!value.ImplementsGenericInterface(typeof(IVerticalReportPostBuilder<>)))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IVerticalReportPostBuilder<>)}");
                }

                base.PostBuilder = value;
            }
        }

        public VerticalReportAttribute()
            : base(ReportType.Vertical)
        {
        }
    }
}
