using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    public class EmptyCellProvider<TSourceEntity> : ReportCellProvider<TSourceEntity, string>
    {
        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(string.Empty);
        }
    }
}
