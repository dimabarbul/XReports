using System;

namespace Reports.Interfaces
{
    public interface IReportColumn<in TSourceEntity>
    {
        string Title { get; }
        Func<TSourceEntity, IReportCell> CellSelector { get; }
    }
}
