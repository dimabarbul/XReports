using System;
using System.Collections.Generic;
using XReports.SchemaBuilders.ReportCellProcessors;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Extensions for report column builder.
    /// </summary>
    public static class ReportColumnBuilderExtensions
    {
        /// <summary>
        /// Adds dynamic property to the report column - the property that depend on data source item.
        /// </summary>
        /// <param name="builder">Report column builder.</param>
        /// <param name="propertySelector">Function that returns property to add to cell based on data source item or null if nothing should be added.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <returns>The report column builder.</returns>
        public static IReportColumnBuilder<TSourceItem> AddDynamicProperties<TSourceItem>(
            this IReportColumnBuilder<TSourceItem> builder,
            Func<TSourceItem, ReportCellProperty> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TSourceItem>(propertySelector));

            return builder;
        }

        /// <summary>
        /// Adds dynamic properties to the report column - the properties that depend on data source item.
        /// </summary>
        /// <param name="builder">Report column builder.</param>
        /// <param name="propertiesSelector">Function that returns properties to add to cell based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <returns>The report column builder.</returns>
        public static IReportColumnBuilder<TSourceItem> AddDynamicProperties<TSourceItem>(
            this IReportColumnBuilder<TSourceItem> builder,
            Func<TSourceItem, IEnumerable<ReportCellProperty>> propertiesSelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TSourceItem>(propertiesSelector));

            return builder;
        }
    }
}
