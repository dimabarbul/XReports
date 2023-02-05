using System;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;

namespace XReports.Extensions
{
    public static class VerticalReportSchemaBuilderExtensions
    {
        public static IReportSchemaCellsProviderBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.AddColumn(title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.AddColumn(title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.AddColumn(id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.AddColumn(id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumn(index, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumn(index, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            int index,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumn(index, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            int index,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumn(index, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeTitle, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeTitle, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeId, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeId, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeTitle, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeTitle, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            ColumnId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertColumnBefore(beforeId, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            ColumnId beforeId,
            ColumnId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertColumnBefore(beforeId, id, title, provider);
        }
    }
}
