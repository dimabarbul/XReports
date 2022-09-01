using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;

namespace XReports.Models
{
    public class ReportTable<TReportCell> : IReportTable<TReportCell>
    {
        public IEnumerable<ReportTableProperty> Properties { get; set; }

        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; set; }

        public IEnumerable<IEnumerable<TReportCell>> Rows { get; set; }

        public bool HasProperty<TProperty>() where TProperty : ReportTableProperty
        {
            return this.HasProperty(typeof(TProperty));
        }

        public bool HasProperty(Type propertyType)
        {
            return this.Properties.Any(p => p.GetType() == propertyType);
        }

        public TProperty GetProperty<TProperty>() where TProperty : ReportTableProperty
        {
            return (TProperty)this.Properties.FirstOrDefault(p => p.GetType() == typeof(TProperty));
        }
    }
}
