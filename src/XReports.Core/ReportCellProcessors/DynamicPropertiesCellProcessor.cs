using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.ReportCellProcessors
{
    public class DynamicPropertiesCellProcessor<TSourceEntity> : IReportCellProcessor<TSourceEntity>
    {
        private readonly Func<TSourceEntity, ReportCellProperty> propertySelector;
        private readonly Func<TSourceEntity, IEnumerable<ReportCellProperty>> propertiesSelector;

        public DynamicPropertiesCellProcessor(Func<TSourceEntity, ReportCellProperty> propertySelector)
        {
            this.propertySelector = propertySelector;
            this.propertiesSelector = null;
        }

        public DynamicPropertiesCellProcessor(Func<TSourceEntity, IEnumerable<ReportCellProperty>> propertySelector)
        {
            this.propertySelector = null;
            this.propertiesSelector = propertySelector;
        }

        public void Process(ReportCell cell, TSourceEntity entity)
        {
            if (this.propertySelector != null)
            {
                ReportCellProperty property = this.propertySelector(entity);
                if (property != null)
                {
                    cell.AddProperty(property);
                }
            }

            if (this.propertiesSelector != null)
            {
                cell.AddProperties(this.propertiesSelector(entity).Where(p => p != null));
            }
        }
    }
}
