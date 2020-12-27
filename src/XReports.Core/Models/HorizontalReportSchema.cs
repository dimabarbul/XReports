using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;

namespace XReports.Models
{
    public class HorizontalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        public ReportSchemaCellsProvider<TSourceEntity>[] HeaderRows { get; protected internal set; }

        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportCell[][] complexHeader = this.CreateComplexHeader(transpose: true);

            return new ReportTable<ReportCell>
            {
                HeaderRows = this.GetHeaderRows(source, complexHeader),
                Rows = this.GetRows(source, complexHeader),
            };
        }

        private IEnumerable<IEnumerable<ReportCell>> GetHeaderRows(IEnumerable<TSourceEntity> source, ReportCell[][] complexHeader)
        {
            return this.HeaderRows
                .Select(row =>
                {
                    ReportCell headerCell = row.CreateHeaderCell();
                    int columnSpan = complexHeader[0].Length;
                    headerCell.ColumnSpan = columnSpan;

                    return new ReportCell[] { headerCell }
                        .Concat(Enumerable.Repeat<ReportCell>(null, columnSpan - 1))
                        .Concat(source.Select(row.CreateCell));
                });
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source, ReportCell[][] complexHeader)
        {
            int rowIndex = 0;

            return this.CellsProviders
                .Select(row =>
                    complexHeader[rowIndex++]
                        .Concat(source.Select(entity => this.AddTableProperties(row.CreateCell(entity))))
                );
        }
    }
}
