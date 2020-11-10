using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports
{
    public class ReportConverter<TResultReportCell>
        where TResultReportCell : BaseReportCell, new()
    {
        private readonly IEnumerable<IPropertyHandler<TResultReportCell>> propertyHandlers;

        public ReportConverter(IEnumerable<IPropertyHandler<TResultReportCell>> propertyHandlers)
        {
            this.propertyHandlers = propertyHandlers;
        }

        public IReportTable<TResultReportCell> Convert(IReportTable<ReportCell> table)
        {
            return new ReportTable<TResultReportCell>
            {
                HeaderRows = this.ConvertHeader(table),
                Rows = this.ConvertBody(table),
            };
        }

        private IEnumerable<IEnumerable<TResultReportCell>> ConvertHeader(IReportTable<ReportCell> table)
        {
            return table.HeaderRows
                .Select(row => row.Select(this.ConvertCell));
        }

        private IEnumerable<IEnumerable<TResultReportCell>> ConvertBody(IReportTable<ReportCell> table)
        {
            return table.Rows
                .Select(row => row.Select(this.ConvertCell));
        }

        private TResultReportCell ConvertCell(ReportCell cell)
        {
            TResultReportCell convertedCell = new TResultReportCell();
            convertedCell.Copy(cell);

            this.ProcessProperties(cell, convertedCell);

            return convertedCell;
        }

        private void ProcessProperties(ReportCell cell, TResultReportCell convertedCell)
        {
            IReportCellProperty[] reportCellProperties = cell.Properties.ToArray();
            foreach (IPropertyHandler<TResultReportCell> handler in this.propertyHandlers
                .OrderBy(h => h.Priority))
            {
                foreach (IReportCellProperty property in reportCellProperties)
                {
                    handler.Handle(property, convertedCell);
                }
            }
        }
    }
}
