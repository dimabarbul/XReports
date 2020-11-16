using System;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellProcessors
{
    public class AddPropertyReportCellProcessor<TSourceEntity> : IReportCellProcessor<TSourceEntity>
    {
        private readonly string title;
        private readonly ReportCellProperty property;

        public AddPropertyReportCellProcessor(string title, ReportCellProperty property)
        {
            this.title = title;
            this.property = property;
        }

        public void Process(ReportCell cell, TSourceEntity entity)
        {
            if (cell.InternalValue.Equals(this.title, StringComparison.OrdinalIgnoreCase))
            {
                cell.AddProperty(this.property);
            }
        }
    }
}
