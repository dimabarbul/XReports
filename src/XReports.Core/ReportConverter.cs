using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports
{
    public class ReportConverter<TResultReportCell> : IReportConverter<TResultReportCell>
        where TResultReportCell : BaseReportCell, new()
    {
        public ReportConverter(IEnumerable<IPropertyHandler<TResultReportCell>> propertyHandlers = null)
        {
            this.Handlers = propertyHandlers ?? Enumerable.Empty<IPropertyHandler<TResultReportCell>>();
        }

        public IEnumerable<IPropertyHandler<TResultReportCell>> Handlers { get; }

        public IReportTable<TResultReportCell> Convert(IReportTable<ReportCell> table)
        {
            return new ReportTable<TResultReportCell>
            {
                Properties = table.Properties,
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
            if (cell == null)
            {
                return null;
            }

            TResultReportCell convertedCell = new TResultReportCell();
            convertedCell.CopyFrom(cell);

            this.ProcessProperties(cell.Properties, convertedCell);

            convertedCell.Properties.AddRange(cell.Properties.Where(p => !p.Processed));

            return convertedCell;
        }

        private void ProcessProperties(List<ReportCellProperty> cellProperties, TResultReportCell convertedCell)
        {
            foreach (IPropertyHandler<TResultReportCell> handler in this.Handlers
                .OrderBy(h => h.Priority))
            {
                foreach (ReportCellProperty property in cellProperties)
                {
                    handler.Handle(property, convertedCell);
                }
            }
        }
    }
}
