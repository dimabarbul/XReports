using System;
using Reports.Core.Models;

namespace Reports.Core.Interfaces
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
