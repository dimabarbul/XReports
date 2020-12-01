using System;
using Reports.Extensions.Builders.Enums;
using Reports.Extensions.Builders.Interfaces;

namespace Reports.Extensions.Builders.Attributes
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
