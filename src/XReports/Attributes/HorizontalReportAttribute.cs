using System;
using XReports.Extensions;
using XReports.Enums;
using XReports.Interfaces;

namespace XReports.Attributes
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
