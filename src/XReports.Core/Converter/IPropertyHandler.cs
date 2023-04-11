using XReports.Table;

namespace XReports.Converter
{
    /// <summary>
    /// Interface for class to handle cell property during report conversion.
    /// </summary>
    /// <seealso cref="IReportConverter{TResultReportCell}"/>
    /// <typeparam name="TResultReportCell">Type of report cell that the handler can work with.</typeparam>
    public interface IPropertyHandler<in TResultReportCell>
        where TResultReportCell : ReportCell
    {
        /// <summary>
        /// Gets priority of the handler. The lower priority the earlier handler is called.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Handles cell property. Will be called for each property for each cell.
        /// </summary>
        /// <param name="property">Property to handle.</param>
        /// <param name="cell">Cell with the property.</param>
        /// <returns>True if the property is handled and requires no further processing, false otherwise.</returns>
        bool Handle(ReportCellProperty property, TResultReportCell cell);
    }
}
