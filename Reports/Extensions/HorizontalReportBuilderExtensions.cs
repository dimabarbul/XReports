using System;
using Reports.Builders;
using Reports.Interfaces;
using Reports.ReportCellsProviders;

namespace Reports.Extensions
{
    public static class HorizontalReportBuilderExtensions
    {
        public static IReportCellsProvider<TEntity, TValue> AddRow<TEntity, TValue>(
            this HorizontalReportBuilder<TEntity> builder, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);
            builder.AddRow(provider);

            return provider;
        }

        public static IReportCellsProvider<TEntity, TValue> AddRow<TEntity, TValue>(
            this HorizontalReportBuilder<TEntity> builder, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);
            builder.AddRow(provider);

            return provider;
        }

        public static IReportCellsProvider<TEntity, TValue> AddRow<TEntity, TValue>(
            this HorizontalReportBuilder<TEntity> builder, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);
            builder.AddRow(provider);

            return provider;
        }

        public static IReportCellsProvider<TEntity, TValue> AddHeaderRow<TEntity, TValue>(
            this HorizontalReportBuilder<TEntity> builder, int rowIndex, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider =
                new EntityPropertyReportCellsProvider<TEntity, TValue>(title, valueSelector);
            builder.AddHeaderRow(rowIndex, provider);

            return provider;
        }

        public static IReportCellsProvider<TEntity, TValue> AddHeaderRow<TEntity, TValue>(
            this HorizontalReportBuilder<TEntity> builder, int rowIndex, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);
            builder.AddHeaderRow(rowIndex, provider);

            return provider;
        }
    }
}
