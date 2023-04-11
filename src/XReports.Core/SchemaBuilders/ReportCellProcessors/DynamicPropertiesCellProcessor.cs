using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProcessors
{
    /// <summary>
    /// Cells processor that adds properties based on data source item.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public class DynamicPropertiesCellProcessor<TSourceItem> : IReportCellProcessor<TSourceItem>
    {
        private readonly Func<TSourceItem, ReportCellProperty> propertySelector;
        private readonly Func<TSourceItem, IEnumerable<ReportCellProperty>> propertiesSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPropertiesCellProcessor{TSourceItem}"/> class.
        /// </summary>
        /// <param name="propertySelector">Function that returns single property to add or null if nothing should be added.</param>
        public DynamicPropertiesCellProcessor(Func<TSourceItem, ReportCellProperty> propertySelector)
        {
            this.propertySelector = propertySelector;
            this.propertiesSelector = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPropertiesCellProcessor{TSourceItem}"/> class.
        /// </summary>
        /// <param name="propertiesSelector">Function that returns properties to add. Properties equal to null are ignored.</param>
        public DynamicPropertiesCellProcessor(Func<TSourceItem, IEnumerable<ReportCellProperty>> propertiesSelector)
        {
            this.propertySelector = null;
            this.propertiesSelector = propertiesSelector;
        }

        /// <inheritdoc />
        public void Process(ReportCell cell, TSourceItem item)
        {
            if (this.propertySelector != null)
            {
                ReportCellProperty property = this.propertySelector(item);
                if (property != null)
                {
                    cell.AddProperty(property);
                }
            }

            if (this.propertiesSelector != null)
            {
                cell.AddProperties(this.propertiesSelector(item).Where(p => p != null));
            }
        }
    }
}
