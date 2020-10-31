using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Cells;

namespace Reports.Builders
{
    public partial class HorizontalReportBuilder<TSourceEntity>
    {
        private readonly List<IReportCellsProvider<TSourceEntity>> rows = new List<IReportCellsProvider<TSourceEntity>>();

        public IReportCellsProvider<TSourceEntity> AddRow(IReportCellsProvider<TSourceEntity> provider)
        {
            this.rows.Add(provider);

            return provider;
        }

        public IReportCellsProvider<TSourceEntity> GetRow(string title)
        {
            return this.rows
                .First(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public IReportCellsProvider<TSourceEntity, TValue> GetTypedRow<TValue>(string title)
        {
            return this.rows
                .First(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                as IReportCellsProvider<TSourceEntity, TValue>;
        }

        public IReportTable Build(IEnumerable<TSourceEntity> source)
        {
            ReportTable table = new ReportTable();

            table.HeaderRows = Enumerable.Empty<IEnumerable<IReportCell>>();
            this.BuildBody(table, source);

            return table;
        }

        private void BuildBody(ReportTable table, IEnumerable<TSourceEntity> source)
        {
            table.Rows = this.GetRows(source);
        }

        private IEnumerable<IEnumerable<IReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return this.rows
                .Select(row => Enumerable.Repeat(this.CreateTitleCell(row), 1)
                    .Concat(source.Select(e => row.CellSelector(e)))
                );
        }

        private IReportCell CreateTitleCell(IReportCellsProvider<TSourceEntity> row)
        {
            return new ReportCell<string>(row.Title);
        }
    }
}
