using System;
using XReports.Models;

namespace XReports.Attributes
{
    public sealed class CustomPropertyAttribute : BasePropertyAttribute
    {
        public CustomPropertyAttribute(Type propertyType)
        {
            if (!typeof(ReportCellProperty).IsAssignableFrom(propertyType))
            {
                throw new ArgumentException($"Type {propertyType} should derive from {typeof(ReportCellProperty)}");
            }

            this.PropertyType = propertyType;
        }

        public Type PropertyType { get; }
    }
}
