using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports
{
    public class ReportConverter<TResultReportCell> : IReportConverter<TResultReportCell>
        where TResultReportCell : BaseReportCell, new()
    {
        private readonly TResultReportCell resultCell = new TResultReportCell();

        public ReportConverter(IEnumerable<IPropertyHandler<TResultReportCell>> propertyHandlers = null)
        {
            this.Handlers = (
                propertyHandlers?.OrderBy(h => h.Priority) ??
                Enumerable.Empty<IPropertyHandler<TResultReportCell>>())
                .ToArray();
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
            foreach (IEnumerable<ReportCell> row in table.HeaderRows)
            {
                yield return row.Select(this.ConvertCell);
            }
        }

        private IEnumerable<IEnumerable<TResultReportCell>> ConvertBody(IReportTable<ReportCell> table)
        {
            foreach (IEnumerable<ReportCell> row in table.Rows)
            {
                yield return row.Select(this.ConvertCell);
            }
        }

        private TResultReportCell ConvertCell(ReportCell cell)
        {
            if (cell == null)
            {
                return null;
            }

            // TResultReportCell convertedCell = new TResultReportCell();
            TResultReportCell convertedCell = this.resultCell;
            convertedCell.CopyFrom(cell);

            this.ProcessProperties(cell.Properties, convertedCell);

            // convertedCell.Properties.AddRange(cell.Properties.Where(p => !p.Processed));
            foreach (ReportCellProperty property in cell.Properties)
            {
                if (!property.Processed)
                {
                    convertedCell.Properties.Add(property);
                }
            }

            return convertedCell;
        }

        private void ProcessProperties(List<ReportCellProperty> cellProperties, TResultReportCell convertedCell)
        {
            foreach (IPropertyHandler<TResultReportCell> handler in this.Handlers)
            {
                foreach (ReportCellProperty property in cellProperties)
                {
                    handler.Handle(property, convertedCell);
                }
            }
        }
    }
}
