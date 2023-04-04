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
        IEnumerable<ReportTableProperty> Properties { get; }

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
            where TProperty : ReportTableProperty;

        /// <summary>
        /// Checks if the table has property of specified type. Derived types are ignored.
        /// </summary>
        /// <param name="propertyType">Type of property to check for.</param>
        /// <returns>True if the table has property of specified type, false otherwise.</returns>
        bool HasProperty(Type propertyType);

        /// <summary>
        /// Returns first table property of type <typeparamref name="TProperty"/> or null if there is no such property. Derived types are ignored.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to get.</typeparam>
        /// <returns>First table property of type <typeparamref name="TProperty"/> or null.</returns>
        TProperty GetProperty<TProperty>()
            where TProperty : ReportTableProperty;
    }
}
