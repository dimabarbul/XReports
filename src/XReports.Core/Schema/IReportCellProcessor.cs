using XReports.Table;

namespace XReports.Schema
{
    public interface IReportCellProcessor<in TSourceEntity>
    {
        void Process(ReportCell cell, TSourceEntity entity);
    }
}
