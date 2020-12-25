using System;
using Reports.Enums;
using Reports.Extensions;
using Reports.Interfaces;

namespace Reports.Attributes
{
    public class HorizontalReportAttribute : ReportAttribute
    {
        public override Type PostBuilder
        {
            get => base.PostBuilder;
            set
            {
                if (!value.ImplementsGenericInterface(typeof(IHorizontalReportPostBuilder<>)))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IHorizontalReportPostBuilder<>)}");
                }

                base.PostBuilder = value;
            }
        }

        public HorizontalReportAttribute()
            : base(ReportType.Horizontal)
        {
        }
    }
}
