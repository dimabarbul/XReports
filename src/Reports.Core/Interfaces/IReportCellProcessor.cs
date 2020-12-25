using Reports.Models;

namespace Reports.Interfaces
{
    public interface IReportCellProcessor<in TSourceEntity>
    {
        void Process(ReportCell cell, TSourceEntity entity);
    }
}
