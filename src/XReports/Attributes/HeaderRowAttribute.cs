using System;

namespace XReports.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HeaderRowAttribute : Attribute
    {
        public HeaderRowAttribute(int order, string title)
        {
            this.Order = order;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        public int Order { get; }

        public string Title { get; }
    }
}
