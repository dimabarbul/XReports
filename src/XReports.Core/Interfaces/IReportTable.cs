using System;
using System.Collections.Generic;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportTable<out TReportCell>
    {
        IEnumerable<ReportTableProperty> Properties { get; }

        IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; }

        IEnumerable<IEnumerable<TReportCell>> Rows { get; }

        bool HasProperty<TProperty>()
            where TProperty : ReportTableProperty;

        bool HasProperty(Type propertyType);

        TProperty GetProperty<TProperty>()
            where TProperty : ReportTableProperty;
    }
}
