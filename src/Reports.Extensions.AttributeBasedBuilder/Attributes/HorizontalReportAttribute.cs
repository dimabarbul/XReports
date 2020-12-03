using System;
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
                if (!typeof(IHorizontalReportPostBuilder).IsAssignableFrom(value))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IHorizontalReportPostBuilder)}");
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
