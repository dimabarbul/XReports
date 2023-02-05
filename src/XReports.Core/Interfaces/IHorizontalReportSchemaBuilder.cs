using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHorizontalReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddRow(string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRow(int index, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(RowId beforeId, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> AddRow(RowId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRow(int index, RowId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(string beforeTitle, RowId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(RowId beforeId, RowId id, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(string title);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(int index);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(RowId id);

        IReportSchemaCellsProviderBuilder<TSourceEntity> AddHeaderRow(string title,
            IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertHeaderRow(int rowIndex, string title,
            IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForHeaderRow(int index);

        IHorizontalReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, string title, RowId fromColumn, RowId toColumn = null);

        IHorizontalReportSchemaBuilder<TSourceEntity> AddComplexHeader(
            int rowIndex, int rowSpan, string title, RowId fromColumn, RowId toColumn = null);

        HorizontalReportSchema<TSourceEntity> BuildSchema();
    }
}
