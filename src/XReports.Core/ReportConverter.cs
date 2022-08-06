using System.Collections;
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
                HeaderRows = new HeaderRowsEnumerator(table, this),
                Rows = new RowsEnumerator(table, this),
            };
        }

        private TResultReportCell ConvertCell(ReportCell cell)
        {
            if (cell == null)
            {
                return null;
            }

            this.resultCell.CopyFrom(cell);

            this.ProcessProperties(cell.Properties);

            return this.resultCell;
        }

        private void ProcessProperties(IReadOnlyList<ReportCellProperty> cellProperties)
        {
            for (int i = 0; i < cellProperties.Count; i++)
            {
                for (int j = 0; j < this.Handlers.Length; j++)
                {
                    this.Handlers[j].Handle(cellProperties[i], this.resultCell);
                }

                if (!cellProperties[i].Processed)
                {
                    this.resultCell.AddProperty(cellProperties[i]);
                }
            }
        }

        private class HeaderRowsEnumerator : IEnumerable<IEnumerable<TResultReportCell>>, IEnumerator<IEnumerable<TResultReportCell>>
        {
            private readonly IEnumerator<IEnumerable<ReportCell>> tableRowsEnumerator;
            private readonly CellsEnumerator cellsEnumerator;

            public HeaderRowsEnumerator(IReportTable<ReportCell> reportTable, ReportConverter<TResultReportCell> reportConverter)
            {
                this.tableRowsEnumerator = reportTable.HeaderRows.GetEnumerator();
                this.cellsEnumerator = new CellsEnumerator(reportConverter);
            }

            public IEnumerable<TResultReportCell> Current => this.cellsEnumerator.WithRow(this.tableRowsEnumerator.Current);

            object IEnumerator.Current => this.Current;

            public IEnumerator<IEnumerable<TResultReportCell>> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.tableRowsEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.tableRowsEnumerator.Reset();
            }

            public void Dispose()
            {
                this.tableRowsEnumerator.Dispose();
                this.cellsEnumerator.Dispose();
            }
        }

        private class RowsEnumerator : IEnumerable<IEnumerable<TResultReportCell>>, IEnumerator<IEnumerable<TResultReportCell>>
        {
            private readonly IEnumerator<IEnumerable<ReportCell>> tableRowsEnumerator;
            private readonly CellsEnumerator cellsEnumerator;

            public RowsEnumerator(IReportTable<ReportCell> reportTable, ReportConverter<TResultReportCell> reportConverter)
            {
                this.tableRowsEnumerator = reportTable.Rows.GetEnumerator();
                this.cellsEnumerator = new CellsEnumerator(reportConverter);
            }

            public IEnumerable<TResultReportCell> Current => this.cellsEnumerator.WithRow(this.tableRowsEnumerator.Current);

            object IEnumerator.Current => this.Current;

            public IEnumerator<IEnumerable<TResultReportCell>> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.tableRowsEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.tableRowsEnumerator.Reset();
            }

            public void Dispose()
            {
                this.tableRowsEnumerator.Dispose();
                this.cellsEnumerator.Dispose();
            }
        }

        private class CellsEnumerator : IEnumerable<TResultReportCell>, IEnumerator<TResultReportCell>
        {
            private readonly ReportConverter<TResultReportCell> reportConverter;
            private IEnumerator<ReportCell> enumerator;

            public CellsEnumerator(ReportConverter<TResultReportCell> reportConverter)
            {
                this.reportConverter = reportConverter;
            }

            public TResultReportCell Current => this.reportConverter.ConvertCell(this.enumerator.Current);

            object IEnumerator.Current => this.Current;

            public CellsEnumerator WithRow(IEnumerable<ReportCell> row)
            {
                this.enumerator?.Dispose();

                this.enumerator = row.GetEnumerator();

                return this;
            }

            public IEnumerator<TResultReportCell> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator?.Dispose();
            }
        }
    }
}
