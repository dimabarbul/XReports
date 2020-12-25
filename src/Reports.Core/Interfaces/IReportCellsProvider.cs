using System;
using Reports.Models;

namespace Reports.Interfaces
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
