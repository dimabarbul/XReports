using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportCellProcessor<in TSourceEntity>
    {
        void Process(ReportCell cell, TSourceEntity entity);
    }
}
