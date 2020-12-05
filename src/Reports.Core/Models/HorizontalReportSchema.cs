using System.Collections.Generic;
using System.Linq;
using Reports.Core.Interfaces;

namespace Reports.Core.Models
{
    public class HorizontalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        public ReportSchemaCellsProvider<TSourceEntity>[] HeaderRows { get; protected internal set; }

        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            return new ReportTable<ReportCell>
            {
                HeaderRows = this.GetHeaderRows(source),
                Rows = this.GetRows(source),
            };
        }

        private IEnumerable<IEnumerable<ReportCell>> GetHeaderRows(IEnumerable<TSourceEntity> source)
        {
            return this.HeaderRows
                .Select(row => new ReportCell[] { row.CreateHeaderCell() }
                    .Concat(source.Select(row.CreateCell))
                );
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return this.CellsProviders
                .Select(row => new ReportCell[] { row.CreateHeaderCell() }
                    .Concat(source.Select(row.CreateCell))
                );
        }
    }
}
