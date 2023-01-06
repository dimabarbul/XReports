using System.Collections.Generic;
using System.Linq;
using XReports.Interfaces;
using XReports.ReportSchemaCellsProviders;

namespace XReports.Models
{
    public class HorizontalReportSchema<TSourceEntity> : ReportSchema<TSourceEntity>
    {
        private readonly ReportSchemaCellsProvider<TSourceEntity>[] headerRows;

        public HorizontalReportSchema(ReportSchemaCellsProvider<TSourceEntity>[] headerRows, ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders, ReportTableProperty[] tableProperties, ComplexHeaderCell[,] complexHeader, Dictionary<string, ReportCellProperty[]> complexHeaderProperties, ReportCellProperty[] commonComplexHeaderProperties)
            : base(cellsProviders, tableProperties, complexHeader, complexHeaderProperties, commonComplexHeaderProperties)
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
            int rowIndex = 0;

            return this.CellsProviders
                .Select(
                    row =>
                        complexHeader[rowIndex++]
                            .Concat(source.Select(row.CreateCell)));
        }
    }
}
