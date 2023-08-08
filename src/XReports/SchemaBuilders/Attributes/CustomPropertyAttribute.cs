using System;
using XReports.Table;

namespace XReports.SchemaBuilders.Attributes
{
    /// <summary>
    /// Attribute that specifies custom property (with parameterless constructor) to assign.
    /// </summary>
    public sealed class CustomPropertyAttribute : BasePropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyType">Type of property to assign. Should have accessible parameterless constructor.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyType"/> is not derived from <see cref="IReportCellProperty"/>.</exception>
        public CustomPropertyAttribute(Type propertyType)
        {
            if (!typeof(IReportCellProperty).IsAssignableFrom(propertyType))
            {
                throw new ArgumentException($"Type {propertyType} should implement {typeof(IReportCellProperty)}");
            }

            this.PropertyType = propertyType;
        }

        /// <summary>
        /// Gets a type of property to assign.
        /// </summary>
        public Type PropertyType { get; }
    }
}
