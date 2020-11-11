using System;

namespace Reports.Extensions.Builders.Attributes
{
    public class ReportVariableAttribute : Attribute
    {
        public int Order { get; set; }
        public string Title { get; set; }

        public ReportVariableAttribute(int order, string title)
        {
            this.Order = order;
            this.Title = title;
        }
    }
}
