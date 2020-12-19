using System;
using Reports.Core.Extensions;
using Reports.Extensions.AttributeBasedBuilder.Enums;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
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
