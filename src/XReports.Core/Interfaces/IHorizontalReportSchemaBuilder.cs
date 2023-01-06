using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHorizontalReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddRow(string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRow(int index, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(string title);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(int index);

        IReportSchemaCellsProviderBuilder<TSourceEntity> AddHeaderRow(string title,
            IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertHeaderRow(int rowIndex, string title,
            IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForHeaderRow(int index);

        HorizontalReportSchema<TSourceEntity> BuildSchema();
    }
}
