using System.Collections.Generic;
using System.Linq;
using XReports.Table;

namespace XReports.Schema
{
    internal class HorizontalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>, IHorizontalReportSchema<TSourceEntity>
    {
        private readonly IReadOnlyList<IReportColumn<TSourceEntity>> headerRows;

        public HorizontalReportSchema(
            IReadOnlyList<IReportColumn<TSourceEntity>> headerRows,
            IReadOnlyList<IReportColumn<TSourceEntity>> columns,
            IReadOnlyList<ReportTableProperty> tableProperties,
            ComplexHeaderCell[,] complexHeader,
            IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties,
            IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties)
            : base(
                columns,
                tableProperties,
                complexHeader,
                complexHeaderProperties,
                commonComplexHeaderProperties)
        {
            this.headerRows = headerRows;
        }

        public override IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportCell[][] complexHeader = this.CreateComplexHeader(isTransposed: true);

            return new ReportTable<ReportCell>
            {
                Properties = this.TableProperties,
                HeaderRows = this.GetHeaderRows(source, complexHeader),
                Rows = this.GetRows(source, complexHeader),
            };
        }

        private IEnumerable<IEnumerable<ReportCell>> GetHeaderRows(IEnumerable<TSourceEntity> source, ReportCell[][] complexHeader)
        {
            return this.headerRows
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
            return this.Columns
                .Select(
                    (row, rowIndex) =>
                        complexHeader[rowIndex]
                            .Concat(source.Select(row.CreateCell)));
        }
    }
}
