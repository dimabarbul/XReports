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
                HeaderRows = new ReportHeaderRows(table, this),
                Rows = new ReportBodyRows(table, this),
                //// HeaderRows = this.ConvertHeader(table),
                //// Rows = this.ConvertBody(table),
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
                    this.resultCell.Properties.Add(cellProperties[i]);
                }
            }
        }

        private class ReportHeaderRows : IEnumerable<IEnumerable<TResultReportCell>>
        {
            private readonly ReportRowEnumerator enumerator;

            public ReportHeaderRows(IReportTable<ReportCell> reportTable, ReportConverter<TResultReportCell> reportConverter)
            {
                this.enumerator = new ReportRowEnumerator(reportTable.HeaderRows.GetEnumerator(), reportConverter);
            }

            public IEnumerator<IEnumerable<TResultReportCell>> GetEnumerator()
            {
                return this.enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class ReportBodyRows : IEnumerable<IEnumerable<TResultReportCell>>
        {
            private readonly ReportRowEnumerator enumerator;

            public ReportBodyRows(IReportTable<ReportCell> reportTable, ReportConverter<TResultReportCell> reportConverter)
            {
                this.enumerator = new ReportRowEnumerator(reportTable.Rows.GetEnumerator(), reportConverter);
            }

            public IEnumerator<IEnumerable<TResultReportCell>> GetEnumerator()
            {
                return this.enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class ReportRowEnumerator : IEnumerator<IEnumerable<TResultReportCell>>
        {
            private readonly IEnumerator<IEnumerable<ReportCell>> enumerator;
            private readonly ReportRow row;

            public ReportRowEnumerator(IEnumerator<IEnumerable<ReportCell>> enumerator, ReportConverter<TResultReportCell> reportConverter)
            {
                this.enumerator = enumerator;
                this.row = new ReportRow(reportConverter);
            }

            public IEnumerable<TResultReportCell> Current
            {
                get
                {
                    this.row.SetRow(this.enumerator.Current);

                    return this.row;
                }
            }

            object IEnumerator.Current => this.Current;

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
                this.enumerator.Dispose();
            }
        }

        private class ReportRow : IEnumerable<TResultReportCell>
        {
            private readonly ReportCellsEnumerator enumerator;

            public ReportRow(ReportConverter<TResultReportCell> reportConverter)
            {
                this.enumerator = new ReportCellsEnumerator(reportConverter);
            }

            public void SetRow(IEnumerable<ReportCell> row)
            {
                this.enumerator.SetRow(row);
            }

            public IEnumerator<TResultReportCell> GetEnumerator()
            {
                return this.enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class ReportCellsEnumerator : IEnumerator<TResultReportCell>
        {
            private readonly ReportConverter<TResultReportCell> reportConverter;
            private IEnumerator<ReportCell> enumerator;

            public ReportCellsEnumerator(ReportConverter<TResultReportCell> reportConverter)
            {
                this.reportConverter = reportConverter;
            }

            public TResultReportCell Current => this.reportConverter.ConvertCell(this.enumerator.Current);

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void SetRow(IEnumerable<ReportCell> row)
            {
                this.enumerator = row.GetEnumerator();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }
        }
    }
}
