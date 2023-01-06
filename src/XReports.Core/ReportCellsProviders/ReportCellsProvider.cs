using XReports.Interfaces;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
        private readonly ReportCell reportCell = new ReportCell();

        public abstract ReportCell GetCell(TSourceEntity entity);

        protected ReportCell CreateCell(TValue value)
        {
            this.reportCell.Clear();
            this.reportCell.SetValue(value);

            return this.reportCell;
        }
    }
}
