using System;
using Reports.Core.Interfaces;
using Reports.Core.ReportCellsProviders;
using Reports.Core.SchemaBuilders;

namespace Reports.Core.Extensions
{
    public static class HorizontalReportSchemaBuilderExtensions
    {
        public static HorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.AddRow(provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.AddRow(provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.AddRow(provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> InsertRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int index, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.InsertRow(index, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> InsertRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int index, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertRow(index, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> InsertRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int index, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.InsertRow(index, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string beforeTitle, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.InsertRowBefore(beforeTitle, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string beforeTitle, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertRowBefore(beforeTitle, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> InsertRowBefore<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string beforeTitle, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.InsertRowBefore(beforeTitle, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddHeaderRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int rowIndex, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider =
                new EntityPropertyReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.AddHeaderRow(rowIndex, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddHeaderRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int rowIndex, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.AddHeaderRow(rowIndex, provider);
        }
    }
}
