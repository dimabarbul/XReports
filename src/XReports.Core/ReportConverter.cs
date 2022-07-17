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

        public IPropertyHandler<TResultReportCell>[] Handlers { get; }

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
                yield return this.GetRow(row);
            }
        }

        private IEnumerable<TResultReportCell> GetRow(IEnumerable<ReportCell> row)
        {
            foreach (ReportCell cell in row)
            {
                yield return this.ConvertCell(cell);
            }
        }

        private IEnumerable<IEnumerable<TResultReportCell>> ConvertBody(IReportTable<ReportCell> table)
        {
            foreach (IEnumerable<ReportCell> row in table.Rows)
            {
                yield return this.GetRow(row);
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

            for (int i = 0; i < cell.Properties.Count; i++)
            {
                if (!cell.Properties[i].Processed)
                {
                    convertedCell.Properties.Add(cell.Properties[i]);
                }
            }

            return convertedCell;
        }

        private void ProcessProperties(List<ReportCellProperty> cellProperties, TResultReportCell convertedCell)
        {
            for (int i = 0; i < this.Handlers.Length; i++)
            {
                for (int j = 0; j < cellProperties.Count; j++)
                {
                    this.Handlers[i].Handle(cellProperties[j], convertedCell);
                }
            }
        }
    }
}
