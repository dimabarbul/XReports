using System;
using Reports.Models;

namespace Reports.Attributes
{
    public class CustomPropertyAttribute : AttributeBase
    {
        public Type PropertyType { get; }

        public CustomPropertyAttribute(Type type)
        {
            if (!typeof(ReportCellProperty).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Type {type} should derive from {typeof(ReportCellProperty)}");
            }

            this.PropertyType = type;
        }
    }
}
