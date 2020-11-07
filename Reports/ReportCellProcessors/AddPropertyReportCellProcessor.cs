using System;
using Reports.Interfaces;

namespace Reports.ReportCellProcessors
{
    public class AddPropertyReportCellProcessor : IReportCellProcessor
    {
        private readonly string title;
        private readonly IReportCellProperty property;

        public AddPropertyReportCellProcessor(string title, IReportCellProperty property)
        {
            this.title = title;
            this.property = property;
        }

        public void Process(IReportCell cell)
        {
            if (cell.DisplayValue.Equals(this.title, StringComparison.OrdinalIgnoreCase))
            {
                cell.AddProperty(this.property);
            }
        }
    }
}
