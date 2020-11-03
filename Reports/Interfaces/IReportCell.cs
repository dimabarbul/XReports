using System;
using System.Collections.Generic;

namespace Reports.Interfaces
{
    public interface IReportCell
    {
        string DisplayValue { get; set; }
        Type ValueType { get; }
        int ColumnSpan { get; }
        int RowSpan { get; }

        IEnumerable<IReportCellProperty> Properties { get; }
        bool HasProperty<TProperty>() where TProperty : IReportCellProperty;
        TProperty GetProperty<TProperty>() where TProperty : IReportCellProperty;
        void AddProperty<TProperty>(TProperty property) where TProperty : IReportCellProperty;
    }
}
