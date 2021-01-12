using System;

namespace XReports.Attributes
{
    public class ReportVariableAttribute : Attribute
    {
        public int Order { get; }
        public string Title { get; }
        public string[] ComplexHeader { get; set; } = new string[0];

        public ReportVariableAttribute(int order, string title)
        {
            this.Order = order;
            this.Title = title;
        }
    }
}
