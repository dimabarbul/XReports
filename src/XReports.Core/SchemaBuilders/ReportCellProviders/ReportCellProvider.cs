using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    public abstract class ReportCellProvider<TSourceEntity, TValue> : IReportCellProvider<TSourceEntity>
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
