using System;
using Reports.Extensions.Builders.Enums;
using Reports.Extensions.Builders.Interfaces;

namespace Reports.Extensions.Builders.Attributes
{
    public class VerticalReportAttribute : ReportAttribute
    {
        private Type postBuilder;

        public Type PostBuilder
        {
            get => this.postBuilder;
            set
            {
                if (!typeof(IVerticalReportPostBuilder).IsAssignableFrom(value))
                {
                    throw new ArgumentException($"Type {value} should implement {typeof(IVerticalReportPostBuilder)}");
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
