using XReports.Models;

namespace XReports.Interfaces
{
    public interface IVerticalReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddColumn(string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumn(int index, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(string title);

        IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(int index);

        VerticalReportSchema<TSourceEntity> BuildSchema();
    }
}
