using System.Collections.Generic;
using System.Linq;
using Reports.Core.Interfaces;

namespace Reports.Core.Models
{
    public class VerticalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>
            {
                HeaderRows = this.CreateComplexHeader(),
                Rows = this.GetRows(source),
            };

            return table;
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return source
                .Select(entity => this.CellsProviders
                    .Select(p => this.AddTableProperties(p.CreateCell(entity)))
                );
        }
    }
}
