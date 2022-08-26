using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.ReportCellProcessors
{
    public class DynamicPropertiesCellProcessor<TSourceEntity> : IReportCellProcessor<TSourceEntity>
    {
        private readonly Func<TSourceEntity, IEnumerable<ReportCellProperty>> propertySelector;

        public DynamicPropertiesCellProcessor(Func<TSourceEntity, ReportCellProperty> propertySelector)
        {
            this.propertySelector = x => new[] { propertySelector(x) };
        }

        public DynamicPropertiesCellProcessor(Func<TSourceEntity, IEnumerable<ReportCellProperty>> propertySelector)
        {
            this.propertySelector = propertySelector;
        }

        public void Process(ReportCell cell, TSourceEntity entity)
        {
            cell.AddProperties(this.propertySelector(entity).Where(p => p != null));
        }
    }
}
