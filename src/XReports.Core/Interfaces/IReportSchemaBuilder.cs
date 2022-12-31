using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportSchemaBuilder<out TSourceEntity>
    {
        IReportSchemaBuilder<TSourceEntity> AddAlias(string alias);

        IReportSchemaBuilder<TSourceEntity> AddAlias(string alias, string target);

        IReportSchemaBuilder<TSourceEntity> AddGlobalProperties(params ReportCellProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportTableProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors);

        IReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(
            params IReportCellProcessor<TSourceEntity>[] processors);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, int fromColumn, int? toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, string fromColumn, string toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(
            string title, params ReportCellProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties);
    }
}
