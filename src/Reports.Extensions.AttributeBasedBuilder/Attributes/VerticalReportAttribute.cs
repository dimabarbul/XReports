using System;
using Reports.Extensions.AttributeBasedBuilder.Enums;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
{
    public class VerticalReportAttribute : ReportAttribute
    {
        private Type postBuilder;

        public Type PostBuilder
        {
            get => this.postBuilder;
            set
            {
                if (!value.ImplementsGenericInterface(typeof(IVerticalReportPostBuilder<>)))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IVerticalReportPostBuilder<>)}");
                }

                this.postBuilder = value;
            }
        }

        public VerticalReportAttribute()
            : base(ReportType.Vertical)
        {
        }
    }
}
