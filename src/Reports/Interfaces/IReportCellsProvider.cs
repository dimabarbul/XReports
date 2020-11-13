using System;
using System.Collections.Generic;
using Reports.Models;

namespace Reports.Interfaces
{
    public interface IReportCellsProvider<in TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
    }

    public interface IReportCellsProvider<in TSourceEntity>
    {
        string Title { get; }
        Func<TSourceEntity, ReportCell> CellSelector { get; }
        ICollection<IReportCellProcessor> Processors { get; }
        ICollection<ReportCellProperty> Properties { get; }

        void AddProcessor(IReportCellProcessor processor);
        void AddProperty(ReportCellProperty property);
    }
}
