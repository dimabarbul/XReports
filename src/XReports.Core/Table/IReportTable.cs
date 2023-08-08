using System;
using System.Collections.Generic;

namespace XReports.Table
{
    /// <summary>
    /// Interface for report table with cells of type <typeparamref name="TReportCell"/>.
    /// </summary>
    /// <typeparam name="TReportCell">Type of cells.</typeparam>
    public interface IReportTable<out TReportCell>
        where TReportCell : ReportCell
    {
        /// <summary>
        /// Gets table properties.
        /// </summary>
        IEnumerable<IReportTableProperty> Properties { get; }

        /// <summary>
        /// Gets header rows of the report.
        /// </summary>
        IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; }

        /// <summary>
        /// Gets rows of the report.
        /// </summary>
        IEnumerable<IEnumerable<TReportCell>> Rows { get; }

        /// <summary>
        /// Checks if the table has property of type <typeparamref name="TProperty"/>. Derived types are ignored.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to check for.</typeparam>
        /// <returns>True if the table has property of type <typeparamref name="TProperty"/>, false otherwise.</returns>
        bool HasProperty<TProperty>()
            where TProperty : IReportTableProperty;

        /// <summary>
        /// Checks if the table has property of specified type. Derived types are ignored.
        /// </summary>
        /// <param name="propertyType">Type of property to check for.</param>
        /// <returns>True if the table has property of specified type, false otherwise.</returns>
        bool HasProperty(Type propertyType);

        /// <summary>
        /// Tries to get the first table property of type <typeparamref name="TProperty"/>. Derived types are ignored. Returns flag indicating whether the property was found.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to get.</typeparam>
        /// <param name="property">The first property of type <typeparamref name="TProperty"/> or default value for the <typeparamref name="TProperty"/> if the property was not found.</param>
        /// <returns>True if property was found, false otherwise.</returns>
        bool TryGetProperty<TProperty>(out TProperty property)
            where TProperty : IReportTableProperty;
    }
}
