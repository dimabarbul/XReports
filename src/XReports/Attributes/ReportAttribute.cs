using System;
using XReports.Enums;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReportAttribute : Attribute
    {
        protected ReportAttribute(ReportType type)
        {
            this.Type = type;
        }

        public ReportType Type { get; }

        public virtual Type PostBuilder { get; set; }
    }
}
