using System;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportSchemaBuilder<out TSourceEntity>
    {
        public IReportSchemaBuilder<TSourceEntity> AddAlias(string alias);

        public IReportSchemaBuilder<TSourceEntity> AddAlias(string alias, string target);

        public IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportCellProperty[] properties);

        public IReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties);

        public IReportSchemaBuilder<TSourceEntity> AddDynamicProperty(
            Func<TSourceEntity, ReportCellProperty> propertySelector);

        public IReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties);

        public IReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors);

        public IReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(
            params IReportCellProcessor<TSourceEntity>[] processors);

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, int fromColumn, int? toColumn = null);

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, string fromColumn, string toColumn = null);

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(
            string title, params ReportCellProperty[] properties);

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties);
    }
}
