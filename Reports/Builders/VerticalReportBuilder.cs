using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Builders
{
    public partial class VerticalReportBuilder<TSourceEntity>
    {
        private readonly List<IReportCellsProvider<TSourceEntity>> columns = new List<IReportCellsProvider<TSourceEntity>>();
        private readonly List<IReportCellProcessor> headerCellProcessors = new List<IReportCellProcessor>();

        public IReportCellsProvider<TSourceEntity> AddColumn(IReportCellsProvider<TSourceEntity> provider)
        {
            this.columns.Add(provider);

            return provider;
        }

        public IReportCellsProvider<TSourceEntity> GetColumn(string title)
        {
            return this.columns
                .First(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public IReportCellsProvider<TSourceEntity, TValue> GetTypedColumn<TValue>(string title)
        {
            return this.columns
                .First(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                as IReportCellsProvider<TSourceEntity, TValue>
                ?? throw new ArgumentException($"Column {title} is of another type: expected {typeof(IReportCellsProvider<TSourceEntity, TValue>)}");
        }

        public void AddHeaderCellProcessor(IReportCellProcessor processor)
        {
            this.headerCellProcessors.Add(processor);
        }

        public IReportTable Build(IEnumerable<TSourceEntity> source)
        {
            ReportTable table = new ReportTable();

            this.BuildHeader(table);
            this.BuildBody(table, source);

            return table;
        }

        private void BuildBody(ReportTable table, IEnumerable<TSourceEntity> source)
        {
            table.Rows = this.GetRows(source);
        }

        private IEnumerable<IEnumerable<IReportCell>> GetRows(IEnumerable<TSourceEntity> source)
        {
            return source
                .Select(entity => this.columns
                    .Select(c => c.CellSelector(entity))
                );
        }
    }
}
