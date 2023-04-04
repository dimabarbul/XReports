using System;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.SchemaBuilders.ReportCellProviders.ValueProviders;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Extensions for report schema builder.
    /// </summary>
    public static class ReportSchemaBuilderExtensions
    {
        /// <summary>
        /// Adds column to the end of the report. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for added column.</returns>
        public static IReportColumnBuilder<TSourceItem> AddColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.AddColumn(title, provider);
        }

        /// <summary>
        /// Adds column to the end of the report. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for added column.</returns>
        public static IReportColumnBuilder<TSourceItem> AddColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.AddColumn(title, provider);
        }

        /// <summary>
        /// Adds column to the end of the report. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="id">Report column identifier.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for added column.</returns>
        public static IReportColumnBuilder<TSourceItem> AddColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            ColumnId id,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.AddColumn(id, title, provider);
        }

        /// <summary>
        /// Adds column to the end of the report. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="id">Report column identifier.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for added column.</returns>
        public static IReportColumnBuilder<TSourceItem> AddColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.AddColumn(id, title, provider);
        }

        /// <summary>
        /// Inserts column at specified position. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="index">0-based index to insert column at.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            int index,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.InsertColumn(index, title, provider);
        }

        /// <summary>
        /// Inserts column at specified position. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="index">0-based index to insert column at.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            int index,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.InsertColumn(index, title, provider);
        }

        /// <summary>
        /// Inserts column at specified position. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="index">0-based index to insert column at.</param>
        /// <param name="id">Report column identifier.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            int index,
            ColumnId id,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.InsertColumn(index, id, title, provider);
        }

        /// <summary>
        /// Inserts column at specified position. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="index">0-based index to insert column at.</param>
        /// <param name="id">Report column identifier.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumn<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            int index,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.InsertColumn(index, id, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified title. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeTitle">Title of existing column to insert new column before.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            string beforeTitle,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeTitle, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified title. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeTitle">Title of existing column to insert new column before.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            string beforeTitle,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeTitle, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified identifier. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeId">Identifier of existing column to insert new column before.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            ColumnId beforeId,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeId, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified identifier. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeId">Identifier of existing column to insert new column before.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            ColumnId beforeId,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeId, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified title. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeTitle">Title of existing column to insert new column before.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            string beforeTitle,
            ColumnId id,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeTitle, id, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified title. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeTitle">Title of existing column to insert new column before.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            string beforeTitle,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeTitle, id, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified identifier. Column cells values will be provided <paramref name="valueSelector"/> based on data source item.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeId">Identifier of existing column to insert new column before.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueSelector">Function to get report cell value based on data source item.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            ColumnId beforeId,
            ColumnId id,
            string title,
            Func<TSourceItem, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TSourceItem, TValue> provider = new ComputedValueReportCellProvider<TSourceItem, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeId, id, title, provider);
        }

        /// <summary>
        /// Inserts column before existing column with specified identifier. Column cells values will be provided <paramref name="valueProvider"/>.
        /// </summary>
        /// <param name="builder">Report schema builder.</param>
        /// <param name="beforeId">Identifier of existing column to insert new column before.</param>
        /// <param name="id">Identifier of the column.</param>
        /// <param name="title">Report column title.</param>
        /// <param name="valueProvider">Provider of cell values.</param>
        /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
        /// <typeparam name="TValue">Type of cell value.</typeparam>
        /// <returns>Builder for inserted column.</returns>
        public static IReportColumnBuilder<TSourceItem> InsertColumnBefore<TSourceItem, TValue>(
            this IReportSchemaBuilder<TSourceItem> builder,
            ColumnId beforeId,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TSourceItem, TValue> provider = new ValueProviderReportCellProvider<TSourceItem, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeId, id, title, provider);
        }
    }
}
