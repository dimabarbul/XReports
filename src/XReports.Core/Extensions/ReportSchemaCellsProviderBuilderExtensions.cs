using System;
using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellProcessors;

namespace XReports.Extensions
{
    public static class ReportSchemaCellsProviderBuilderExtensions
    {
        public static IReportSchemaCellsProviderBuilder<TEntity> AddDynamicProperties<TEntity>(
            this IReportSchemaCellsProviderBuilder<TEntity> builder,
            Func<TEntity, ReportCellProperty> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TEntity>(propertySelector));

            return builder;
        }

        public static IReportSchemaCellsProviderBuilder<TEntity> AddDynamicProperties<TEntity>(
            this IReportSchemaCellsProviderBuilder<TEntity> builder,
            Func<TEntity, IEnumerable<ReportCellProperty>> propertySelector)
        {
            builder.AddProcessors(new DynamicPropertiesCellProcessor<TEntity>(propertySelector));

            return builder;
        }
    }
}
