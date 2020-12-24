using System;
using System.Collections.Generic;
using Reports.Core.Interfaces;
using Reports.Core.Models;

namespace Reports.Core.ReportCellProcessors
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
            foreach (ReportCellProperty property in this.propertySelector(entity))
            {
                cell.AddProperty(property);
            }
        }
    }
}
