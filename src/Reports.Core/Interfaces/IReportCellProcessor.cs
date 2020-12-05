using Reports.Core.Models;

namespace Reports.Core.Interfaces
{
    public interface IReportCellProcessor<in TSourceEntity>
    {
        void Process(ReportCell cell, TSourceEntity entity);
    }
}
