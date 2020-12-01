using System;
using Reports.Extensions.Builders.Enums;

namespace Reports.Extensions.Builders.Attributes
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
