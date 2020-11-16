using System;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellProcessors
{
    public class AddPropertyReportCellProcessor<TSourceEntity> : IReportCellProcessor<TSourceEntity>
    {
        private readonly string title;
        private readonly ReportCellProperty[] properties;

        public AddPropertyReportCellProcessor(string title, params ReportCellProperty[] property)
        {
            this.title = title;
            this.properties = property;
        }

        public void Process(ReportCell cell, TSourceEntity entity)
        {
            if (cell.InternalValue.Equals(this.title, StringComparison.OrdinalIgnoreCase))
            {
                foreach (ReportCellProperty property in this.properties)
                {
                    cell.AddProperty(property);
                }
            }
        }
    }
}
