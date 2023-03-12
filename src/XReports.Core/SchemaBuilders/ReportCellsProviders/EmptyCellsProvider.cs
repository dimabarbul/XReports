using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellsProviders
{
    public class EmptyCellsProvider<TSourceEntity> : ReportCellsProvider<TSourceEntity, string>
    {
        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(string.Empty);
        }
    }
}
