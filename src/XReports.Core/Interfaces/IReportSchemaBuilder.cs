using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportSchemaBuilder<out TSourceEntity>
    {
        IReportSchemaBuilder<TSourceEntity> AddGlobalProperties(params ReportCellProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportTableProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, int fromColumn, int? toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, string fromColumn, string toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(
            string title, params ReportCellProperty[] properties);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties);
    }
}
