using System;
using XReports.Enums;
using XReports.Extensions;
using XReports.Interfaces;

namespace XReports.Attributes
{
    public class HorizontalReportAttribute : ReportAttribute
    {
        public HorizontalReportAttribute()
            : base(ReportType.Horizontal)
        {
        }

        public override Type PostBuilder
        {
            get => base.PostBuilder;
            set
            {
                if (!value.ImplementsGenericInterface(typeof(IHorizontalReportPostBuilder<>))
                    && !value.ImplementsGenericInterface(typeof(IHorizontalReportPostBuilder<,>)))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IHorizontalReportPostBuilder<>)} or {typeof(IHorizontalReportPostBuilder<,>)}");
                }

                base.PostBuilder = value;
            }
        }
    }
}
