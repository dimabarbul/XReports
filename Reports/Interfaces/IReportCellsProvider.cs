using System;
using System.Collections.Generic;

namespace Reports.Interfaces
{
    public interface IReportCellsProvider<in TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
        void SetValueFormatter(IValueFormatter<TValue> formatter);
    }

    public interface IReportCellsProvider<in TSourceEntity>
    {
        string Title { get; }
        Func<TSourceEntity, IReportCell> CellSelector { get; }
        ICollection<IReportCellProcessor> Processors { get; }
        ICollection<IReportCellProperty> Properties { get; }

        void AddProcessor(IReportCellProcessor processor);
        void AddProperty(IReportCellProperty property);
    }
}
