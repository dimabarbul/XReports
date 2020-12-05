using System;
using Reports.Core.Models;

namespace Reports.Extensions.AttributeBasedBuilder.Attributes
{
    public class CustomPropertyAttribute : Attribute
    {
        public Type PropertyType { get; }
        public bool IsHeader { get; set; }

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
