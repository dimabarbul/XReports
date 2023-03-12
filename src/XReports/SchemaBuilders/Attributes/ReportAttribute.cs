using System;

namespace XReports.SchemaBuilders.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReportAttribute : Attribute
    {
        internal ReportAttribute(ReportType type)
        {
            this.Type = type;
        }

        internal ReportType Type { get; }

        public virtual Type PostBuilder { get; set; }
    }
}
