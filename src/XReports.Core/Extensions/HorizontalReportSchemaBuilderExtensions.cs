using System;
using XReports.Interfaces;
using XReports.ReportCellsProviders;

namespace XReports.Extensions
{
    public static class HorizontalReportSchemaBuilderExtensions
    {
        public static IHorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.AddRow(provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.AddRow(provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> InsertRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.InsertRow(index, provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> InsertRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertRow(index, provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.InsertRowBefore(beforeTitle, provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertRowBefore(beforeTitle, provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> InsertHeaderRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            int rowIndex,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider =
                new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.InsertHeaderRow(rowIndex, provider);
        }

        public static IHorizontalReportSchemaBuilder<TEntity> AddHeaderRow<TEntity, TValue>(
            this IHorizontalReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider =
                new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.AddHeaderRow(provider);
        }
    }
}
