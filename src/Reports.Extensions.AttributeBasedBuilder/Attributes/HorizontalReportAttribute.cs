using System;
using Reports.Core.Extensions;
using Reports.Extensions.AttributeBasedBuilder.Enums;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
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
