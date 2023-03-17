using System.Collections.Generic;
using System.Linq;
using XReports.Helpers;
using XReports.Table;

namespace XReports.Schema
{
    internal class HorizontalReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        private readonly IReadOnlyList<IReportColumn<TSourceEntity>> headerRows;
        private readonly IReadOnlyList<IReportColumn<TSourceEntity>> columns;
        private readonly IReadOnlyList<ReportTableProperty> tableProperties;
        private readonly ComplexHeaderCell[,] complexHeader;
        private readonly IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties;
        private readonly IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties;

        public HorizontalReportSchema(
            IReadOnlyList<IReportColumn<TSourceEntity>> headerRows,
            IReadOnlyList<IReportColumn<TSourceEntity>> columns,
            IReadOnlyList<ReportTableProperty> tableProperties,
            ComplexHeaderCell[,] complexHeader,
            IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties,
            IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties)
        {
            this.headerRows = headerRows;
            this.columns = columns;
            this.tableProperties = tableProperties;
            this.complexHeader = complexHeader;
            this.complexHeaderProperties = complexHeaderProperties;
            this.commonComplexHeaderProperties = commonComplexHeaderProperties;
        }

        public IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source)
        {
            ReportCell[][] complexHeader = ComplexHeaderHelper.CreateCells(
                this.columns,
                this.complexHeader,
                this.complexHeaderProperties,
                this.commonComplexHeaderProperties,
                isTransposed: true);

            return new ReportTable<ReportCell>
            {
                Properties = this.tableProperties,
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
            return this.columns
                .Select(
                    (row, rowIndex) =>
                        complexHeader[rowIndex]
                            .Concat(source.Select(row.CreateCell)));
        }
    }
}
