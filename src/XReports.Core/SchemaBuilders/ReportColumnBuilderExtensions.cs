using System;
using System.Collections.Generic;
using XReports.SchemaBuilders.ReportCellProcessors;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    public static class ReportColumnBuilderExtensions
    {
        public static IReportColumnBuilder<TEntity> AddDynamicProperties<TEntity>(
            this IReportColumnBuilder<TEntity> builder,
            Func<TEntity, ReportCellProperty> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TEntity>(propertySelector));

            return builder;
        }

        public static IReportColumnBuilder<TEntity> AddDynamicProperties<TEntity>(
            this IReportColumnBuilder<TEntity> builder,
            Func<TEntity, IEnumerable<ReportCellProperty>> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TEntity>(propertySelector));

            return builder;
        }
    }
}
