using System;
using Reports.Core.Extensions;
using Reports.Extensions.AttributeBasedBuilder.Enums;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
{
    public class HorizontalReportAttribute : ReportAttribute
    {
        private Type postBuilder;

        public Type PostBuilder
        {
            get => this.postBuilder;
            set
            {
                if (!value.ImplementsGenericInterface(typeof(IHorizontalReportPostBuilder<>)))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IHorizontalReportPostBuilder<>)}");
                }

                this.postBuilder = value;
            }
        }

        public HorizontalReportAttribute()
            : base(ReportType.Horizontal)
        {
        }
    }
}
