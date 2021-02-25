using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportTable<out TReportCell>
    {
        public IEnumerable<ReportTableProperty> Properties { get; }

        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; }

        public IEnumerable<IEnumerable<TReportCell>> Rows { get; }

        public bool HasProperty<TProperty>()
            where TProperty : ReportTableProperty
        {
            return this.Properties.OfType<TProperty>().Any();
        }

        public bool HasProperty(Type propertyType)
        {
            return this.Properties.Any(p => p.GetType() == propertyType);
        }

        public TProperty GetProperty<TProperty>()
            where TProperty : ReportTableProperty
        {
            return this.Properties.OfType<TProperty>().FirstOrDefault();
        }
    }
}
