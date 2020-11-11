using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;
using Reports.ReportCellProcessors;

namespace Reports.Builders
{
    public class HorizontalReportBuilder<TSourceEntity>
    {
        private readonly List<IReportCellsProvider<TSourceEntity>> headerRows = new List<IReportCellsProvider<TSourceEntity>>();
        private readonly List<IReportCellsProvider<TSourceEntity>> rows = new List<IReportCellsProvider<TSourceEntity>>();
        private readonly List<IReportCellProcessor> titleCellProcessors = new List<IReportCellProcessor>();

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
            return new ReportTable<ReportCell>
            {
                HeaderRows = this.GetHeaderRows(source),
                Rows = this.GetRows(source),
            };
        }

        public void AddTitleProperty(string title, IReportCellProperty property)
        {
            this.titleCellProcessors.Add(new AddPropertyReportCellProcessor(title, property));
        }

        public void AddHeaderRow(int rowIndex, IReportCellsProvider<TSourceEntity> provider)
        {
            this.headerRows.Insert(rowIndex, provider);
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
            ReportCell<string> cell = new ReportCell<string>(row.Title);

            foreach (IReportCellProcessor reportCellProcessor in this.titleCellProcessors)
            {
                reportCellProcessor.Process(cell);
            }

            return cell;
        }

        private IEnumerable<IEnumerable<ReportCell>> GetHeaderRows(IEnumerable<TSourceEntity> source)
        {
            return this.headerRows
                .Select(row => Enumerable.Repeat(this.CreateTitleCell(row), 1)
                    .Concat(source.Select(e => row.CellSelector(e)))
                );
        }
    }
}
