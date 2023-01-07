using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class EmptyCellsProvider<TSourceEntity> : ReportCellsProvider<TSourceEntity, string>
    {
        private readonly ReportCell reportCell;

        public EmptyCellsProvider()
        {
            this.reportCell = ReportCell.FromValue(string.Empty);
        }

        public override ReportCell GetCell(TSourceEntity entity)
        {
            this.reportCell.Clear();
            this.reportCell.SetValue(string.Empty);

            return this.reportCell;
        }
    }
}
