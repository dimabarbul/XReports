using System;

namespace Reports.Interfaces
{
    public interface IReportCell
    {
        string DisplayValue { get; }
        Type ValueType { get; }
    }
}
