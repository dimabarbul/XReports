using System;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Base attribute that specifies report type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReportAttribute : Attribute
    {
        internal ReportAttribute(ReportType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets type of report schema post-builder class.
        /// </summary>
        public virtual Type PostBuilder { get; set; }

        internal ReportType Type { get; }
    }
}
