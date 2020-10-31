using System;
using Reports.Builders;
using Reports.Interfaces;
using Reports.Models.Columns;

namespace Reports.Extensions
{
    public static class VerticalReportBuilderExtensions
    {
        public static IReportCellsProvider<TEntity, TValue> AddColumn<TEntity, TValue>(
            this VerticalReportBuilder<TEntity> builder, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity,TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);
            builder.AddColumn(provider);

            return provider;
        }

        public static IReportCellsProvider<TEntity, TValue> AddColumn<TEntity, TValue>(
            this VerticalReportBuilder<TEntity> builder, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);
            builder.AddColumn(provider);

            return provider;
        }

        public static IReportCellsProvider<TEntity, TValue> AddColumn<TEntity, TValue>(
            this VerticalReportBuilder<TEntity> builder, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);
            builder.AddColumn(provider);

            return provider;
        }
    }
}
