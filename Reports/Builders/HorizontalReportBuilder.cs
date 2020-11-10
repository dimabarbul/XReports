using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Builders
{
    public class HorizontalReportBuilder<TSourceEntity>
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
                as IReportCellsProvider<TSourceEntity, TValue>
                ?? throw new ArgumentException($"Column {title} is of another type: expected {typeof(IReportCellsProvider<TSourceEntity, TValue>)}");
        }

        public IReportTable<ReportCell> Build(IEnumerable<TSourceEntity> source)
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>();

            table.HeaderRows = Enumerable.Empty<IEnumerable<ReportCell>>();
            table.Rows = this.GetRows(source);

            return table;
        }

        private IEnumerable<IEnumerable<ReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return this.rows
                .Select(row => Enumerable.Repeat(this.CreateTitleCell(row), 1)
                    .Concat(source.Select(e => row.CellSelector(e)))
                );
        }

        private ReportCell CreateTitleCell(IReportCellsProvider<TSourceEntity> row)
        {
            return new ReportCell<string>(row.Title);
        }
    }
}
