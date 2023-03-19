using System;
using System.Collections.Generic;

namespace XReports.Table
{
    public interface IReportTable<out TReportCell>
        where TReportCell : ReportCell
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
