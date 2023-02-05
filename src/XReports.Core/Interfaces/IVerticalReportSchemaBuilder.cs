using XReports.Models;

namespace XReports.Interfaces
{
    public interface IVerticalReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddColumn(string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumn(int index, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> AddColumn(ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumn(int index, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(string title);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(int index);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(ColumnId id);

        IVerticalReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, ColumnId fromColumn, ColumnId toColumn = null);

        IVerticalReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, int rowSpan, string title, ColumnId fromColumn, ColumnId toColumn = null);

        VerticalReportSchema<TSourceEntity> BuildSchema();
    }
}
