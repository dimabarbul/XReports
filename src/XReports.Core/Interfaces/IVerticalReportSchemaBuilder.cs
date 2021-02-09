using XReports.Models;

namespace XReports.Interfaces
{
    public interface IVerticalReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        IVerticalReportSchemaBuilder<TSourceEntity> AddColumn(IReportCellsProvider<TSourceEntity> provider);

        IVerticalReportSchemaBuilder<TSourceEntity> InsertColumn(int index, IReportCellsProvider<TSourceEntity> provider);

        IVerticalReportSchemaBuilder<TSourceEntity> InsertColumnBefore(string title, IReportCellsProvider<TSourceEntity> provider);

        IVerticalReportSchemaBuilder<TSourceEntity> ForColumn(string title);

        VerticalReportSchema<TSourceEntity> BuildSchema();
    }
}
