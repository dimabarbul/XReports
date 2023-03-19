using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    public interface IReportSchemaBuilder<TSourceEntity>
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

        IReportColumnBuilder<TSourceEntity> AddColumn(string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> InsertColumn(int index, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> AddColumn(ColumnId id, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> InsertColumn(int index, ColumnId id, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, ColumnId id, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, ColumnId id, string title, IReportCellProvider<TSourceEntity> provider);

        IReportColumnBuilder<TSourceEntity> ForColumn(string title);

        IReportColumnBuilder<TSourceEntity> ForColumn(int index);

        IReportColumnBuilder<TSourceEntity> ForColumn(ColumnId id);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, ColumnId fromColumn, ColumnId toColumn = null);

        IReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, int rowSpan, string title, ColumnId fromColumn, ColumnId toColumn = null);

        IReportSchema<TSourceEntity> BuildVerticalSchema();

        IReportSchema<TSourceEntity> BuildHorizontalSchema(int headerRowsCount);
    }
}
