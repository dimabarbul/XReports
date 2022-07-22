using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class EmptyCellsProvider<TSourceEntity> : ReportCellsProvider<TSourceEntity, string>
    {
        public EmptyCellsProvider(string title)
            : base(title)
        {
        }

        public override ReportCell GetCell(TSourceEntity entity)
        {
            return ReportCell.EmptyCell;
        }
    }
}
