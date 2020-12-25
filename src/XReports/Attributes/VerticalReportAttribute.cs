using System;
using XReports.Extensions;
using XReports.Enums;
using XReports.Interfaces;

namespace XReports.Attributes
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
