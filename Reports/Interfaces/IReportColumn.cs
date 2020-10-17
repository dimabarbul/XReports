using System;
using System.Collections.Generic;

namespace Reports.Interfaces
{
    public interface IReportColumn<in TSourceEntity, TValue> : IReportColumn<TSourceEntity>
    {
        void SetValueFormatter(IValueFormatter<TValue> formatter);
    }

    public interface IReportColumn<in TSourceEntity>
    {
        string Title { get; }
        Func<TSourceEntity, IReportCell> CellSelector { get; }
        ICollection<IReportCellProcessor> Processors { get; }
    }
}
