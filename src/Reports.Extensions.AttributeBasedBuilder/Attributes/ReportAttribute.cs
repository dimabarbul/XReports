using System;
using Reports.Extensions.AttributeBasedBuilder.Enums;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReportAttribute : Attribute
    {
        public ReportType Type { get; }

        public ReportAttribute(ReportType type)
        {
            this.Type = type;
        }
    }
}
