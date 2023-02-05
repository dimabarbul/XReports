using System;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;

namespace XReports.Extensions
{
    public static class HorizontalReportSchemaBuilderExtensions
    {
        public static IReportSchemaCellsProviderBuilder<TEntity> AddRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.AddRow(title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.AddRow(title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            RowId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.AddRow(id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            RowId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.AddRow(id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertRow(index, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertRow(index, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int index,
            RowId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertRow(index, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int index,
            RowId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertRow(index, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertRowBefore(beforeTitle, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertRowBefore(beforeTitle, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            RowId beforeId,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertRowBefore(beforeId, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            RowId beforeId,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertRowBefore(beforeId, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            RowId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertRowBefore(beforeTitle, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            RowId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertRowBefore(beforeTitle, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            RowId beforeId,
            RowId id,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertRowBefore(beforeId, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            RowId beforeId,
            RowId id,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(valueProvider);

            return builder.InsertRowBefore(beforeId, id, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> InsertHeaderRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int rowIndex,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider =
                new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.InsertHeaderRow(rowIndex, title, provider);
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddHeaderRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider =
                new ComputedValueReportCellsProvider<TEntity, TValue>(valueSelector);

            return builder.AddHeaderRow(title, provider);
        }
    }
}
