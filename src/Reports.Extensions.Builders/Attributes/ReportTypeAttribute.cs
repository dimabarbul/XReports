using System;
using Reports.Extensions.Builders.Enums;

namespace Reports.Extensions.Builders.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReportTypeAttribute : Attribute
    {
        public ReportType Type { get; }

        public ReportTypeAttribute(ReportType type)
        {
            this.Type = type;
        }
    }
}
