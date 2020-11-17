using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Builders
{
    public class HorizontalReportBuilder<TSourceEntity>
    {
        private readonly List<IReportCellsProvider<TSourceEntity>> headerRows = new List<IReportCellsProvider<TSourceEntity>>();
        private readonly List<IReportCellsProvider<TSourceEntity>> rows = new List<IReportCellsProvider<TSourceEntity>>();
        private readonly List<IReportCellProcessor<TSourceEntity>> titleCellProcessors = new List<IReportCellProcessor<TSourceEntity>>();
        private readonly Dictionary<string, List<ReportCellProperty>> titleCellProperties = new Dictionary<string, List<ReportCellProperty>>();

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

        public void AddTitleProperty(string title, params ReportCellProperty[] properties)
        {
            if (!this.titleCellProperties.ContainsKey(title))
            {
                this.titleCellProperties.Add(title, new List<ReportCellProperty>());
            }

            this.titleCellProperties[title].AddRange(properties);
        }

        public void AddTitleProcessor(IReportCellProcessor<TSourceEntity> processor)
        {
            this.titleCellProcessors.Add(processor);
        }

        public void AddHeaderRow(int rowIndex, IReportCellsProvider<TSourceEntity> provider)
        {
            this.headerRows.Insert(rowIndex, provider);
        }

        public void AddHeaderProperty(string title, params ReportCellProperty[] properties)
        {
            IReportCellsProvider<TSourceEntity> row = this.headerRows.FirstOrDefault(r => r.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (row == null)
            {
                throw new ArgumentException($"Cannot find column {title}");
            }

            foreach (ReportCellProperty property in properties)
            {
                row.AddProperty(property);
            }
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

            if (this.titleCellProperties.ContainsKey(row.Title))
            {
                cell.Properties.AddRange(this.titleCellProperties[row.Title]);
            }

            foreach (IReportCellProcessor<TSourceEntity> reportCellProcessor in this.titleCellProcessors)
            {
                reportCellProcessor.Process(cell, default);
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
