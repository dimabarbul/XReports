using System;
using System.Collections.Generic;
using System.Linq;

namespace XReports.Table
{
    internal class ReportTable<TReportCell> : IReportTable<TReportCell>
        where TReportCell : ReportCell
    {
        public IEnumerable<IReportTableProperty> Properties { get; set; }

        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; set; }

        public IEnumerable<IEnumerable<TReportCell>> Rows { get; set; }

        public bool HasProperty<TProperty>()
            where TProperty : IReportTableProperty
        {
            return this.HasProperty(typeof(TProperty));
        }

        public bool HasProperty(Type propertyType)
        {
            return this.Properties.Any(p => p.GetType() == propertyType);
        }

        public bool TryGetProperty<TProperty>(out TProperty property)
            where TProperty : IReportTableProperty
        {
            property = default;
            foreach (IReportTableProperty tableProperty in this.Properties)
            {
                if (tableProperty.GetType() == typeof(TProperty))
                {
                    property = (TProperty)tableProperty;
                    return true;
                }
            }

            return false;
        }
    }
}
