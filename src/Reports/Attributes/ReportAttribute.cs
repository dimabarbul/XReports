using System;
using Reports.Enums;

namespace Reports.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReportAttribute : Attribute
    {
        public ReportType Type { get; }
        public virtual Type PostBuilder { get; set; }

        public ReportAttribute(ReportType type)
        {
            this.Type = type;
        }
    }
}
