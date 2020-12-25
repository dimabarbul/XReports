using System;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
    }

    public interface IReportCellsProvider<TSourceEntity>
    {
        string Title { get; }
        Func<TSourceEntity, ReportCell> CellSelector { get; }
    }
}
