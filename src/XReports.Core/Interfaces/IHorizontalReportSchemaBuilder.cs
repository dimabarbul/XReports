using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHorizontalReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        IHorizontalReportSchemaBuilder<TSourceEntity> AddRow(IReportCellsProvider<TSourceEntity> provider);

        IHorizontalReportSchemaBuilder<TSourceEntity> InsertRow(int index, IReportCellsProvider<TSourceEntity> provider);

        IHorizontalReportSchemaBuilder<TSourceEntity> InsertRowBefore(string title, IReportCellsProvider<TSourceEntity> provider);

        IHorizontalReportSchemaBuilder<TSourceEntity> ForRow(string title);

        IHorizontalReportSchemaBuilder<TSourceEntity> AddHeaderRow(IReportCellsProvider<TSourceEntity> provider);

        IHorizontalReportSchemaBuilder<TSourceEntity> InsertHeaderRow(int rowIndex, IReportCellsProvider<TSourceEntity> provider);

        IHorizontalReportSchemaBuilder<TSourceEntity> ForHeaderRow(int index);

        HorizontalReportSchema<TSourceEntity> BuildSchema();
    }
}
