using System;
using XReports.SchemaBuilders.ReportCellProviders;

namespace XReports.SchemaBuilders
{
    public static class ReportSchemaBuilderExtensions
    {
        public static IReportColumnBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.AddColumn(title, provider);
        }

        public static IReportColumnBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.AddColumn(title, provider);
        }

        public static IReportColumnBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.AddColumn(id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.AddColumn(id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumn(index, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumn(index, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            int index,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumn(index, id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            int index,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumn(index, id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeTitle, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeTitle, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeId, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeId, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeTitle, id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeTitle, id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellProvider<TEntity, TValue> provider = new ComputedValueReportCellProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeId, id, title, provider);
        }

        public static IReportColumnBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellProvider<TEntity, TValue> provider = new ValueProviderReportCellProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeId, id, title, provider);
        }
    }
}
