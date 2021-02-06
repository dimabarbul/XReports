using System;
using XReports.Models;

namespace XReports.Attributes
{
    public class CustomPropertyAttribute : AttributeBase
    {
        public CustomPropertyAttribute(Type type)
        {
            if (!typeof(ReportCellProperty).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Type {type} should derive from {typeof(ReportCellProperty)}");
            }

            this.PropertyType = type;
        }

        public Type PropertyType { get; }
    }
}
