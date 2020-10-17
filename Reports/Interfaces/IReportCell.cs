using System;

namespace Reports.Interfaces
{
    public interface IReportCell
    {
        string DisplayValue { get; }
        Type ValueType { get; }
        int ColumnSpan { get; }
        int RowSpan { get; }
        bool IsHeader { get; }
    }
}
