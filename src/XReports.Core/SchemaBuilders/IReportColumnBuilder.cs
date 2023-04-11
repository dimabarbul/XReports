using System.Collections.Generic;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Interface for report column builder.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportColumnBuilder<TSourceItem>
    {
        /// <summary>
        /// Adds properties to all cells of the report column.
        /// </summary>
        /// <param name="properties">Properties to add.</param>
        /// <returns>The report column builder.</returns>
        IReportColumnBuilder<TSourceItem> AddProperties(params ReportCellProperty[] properties);

        /// <summary>
        /// Adds properties to header cell of the report column.
        /// </summary>
        /// <param name="properties">Properties to add.</param>
        /// <returns>The report column builder.</returns>
        IReportColumnBuilder<TSourceItem> AddHeaderProperties(params ReportCellProperty[] properties);

        /// <summary>
        /// Adds processors to all cells of the report column.
        /// </summary>
        /// <param name="processors">Processors to add.</param>
        /// <returns>The report column builder.</returns>
        IReportColumnBuilder<TSourceItem> AddProcessors(params IReportCellProcessor<TSourceItem>[] processors);

        /// <summary>
        /// Adds processors to header cell of the report column.
        /// </summary>
        /// <param name="processors">Processors to add.</param>
        /// <returns>The report column builder.</returns>
        IReportColumnBuilder<TSourceItem> AddHeaderProcessors(params IHeaderReportCellProcessor[] processors);

        /// <summary>
        /// Builds report column based on data provided earlier.
        /// </summary>
        /// <param name="globalProperties">Report global properties, will be merged with properties of the report column cells. Are not applied to header cell.</param>
        /// <returns>The report column builder.</returns>
        IReportColumn<TSourceItem> Build(IReadOnlyList<ReportCellProperty> globalProperties);
    }
}
