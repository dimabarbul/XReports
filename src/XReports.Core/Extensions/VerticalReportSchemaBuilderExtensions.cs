using System;
using XReports.Interfaces;
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
    }
}
