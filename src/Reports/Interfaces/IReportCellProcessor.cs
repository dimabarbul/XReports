using Reports.Models;

namespace Reports.Interfaces
{
    public interface IReportCellProcessor
    {
        void Process(ReportCell cell);
    }
}
