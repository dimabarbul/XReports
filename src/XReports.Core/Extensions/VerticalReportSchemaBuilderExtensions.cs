using System;
using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellProcessors;
using XReports.ReportCellsProviders;

namespace XReports.Extensions
{
    public static class VerticalReportSchemaBuilderExtensions
    {
        public static IVerticalReportSchemaBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.AddColumn(provider);
        }

        public static IVerticalReportSchemaBuilder<TEntity> AddColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.AddColumn(provider);
        }

        public static IVerticalReportSchemaBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.InsertColumn(index, provider);
        }

        public static IVerticalReportSchemaBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            int index,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertColumn(index, provider);
        }

        public static IVerticalReportSchemaBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            Func<TEntity, TValue> valueSelector)
        {
            ComputedValueReportCellsProvider<TEntity, TValue> provider = new ComputedValueReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.InsertColumnBefore(beforeTitle, provider);
        }

        public static IVerticalReportSchemaBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            string beforeTitle,
            string title,
            IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity, TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertColumnBefore(beforeTitle, provider);
        }

        public static IVerticalReportSchemaBuilder<TEntity> AddDynamicProperties<TEntity>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            Func<TEntity, ReportCellProperty> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TEntity>(propertySelector));

            return builder;
        }

        public static IVerticalReportSchemaBuilder<TEntity> AddDynamicProperties<TEntity>(
            this IVerticalReportSchemaBuilder<TEntity> builder,
            Func<TEntity, IEnumerable<ReportCellProperty>> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TEntity>(propertySelector));

            return builder;
        }
    }
}
