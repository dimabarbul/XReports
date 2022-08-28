using System;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ReportVariableAttribute : Attribute
    {
        public ReportVariableAttribute(int order, string title)
        {
            this.Order = order;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        public int Order { get; }

        public string Title { get; }

        public string[] ComplexHeader { get; set; } = Array.Empty<string>();
    }
}
