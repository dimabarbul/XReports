using System;
using System.Collections.Generic;
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
        ICollection<IReportCellProcessor<TSourceEntity>> Processors { get; }
        ICollection<ReportCellProperty> Properties { get; }

        IReportCellsProvider<TSourceEntity> AddProcessor(IReportCellProcessor<TSourceEntity> processor);
        IReportCellsProvider<TSourceEntity> AddProperty(ReportCellProperty property);
    }
}
