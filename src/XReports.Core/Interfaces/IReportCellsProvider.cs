using System;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportCellsProvider<in TSourceEntity>
    {
        string Title { get; }
        Func<TSourceEntity, ReportCell> CellSelector { get; }
    }
}
