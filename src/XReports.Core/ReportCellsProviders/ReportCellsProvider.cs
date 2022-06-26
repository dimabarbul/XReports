using System;
using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;
using XReports.Utils;

namespace XReports.ReportCellsProviders
{
    public abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
        private readonly ReportCellsPool pool = new ReportCellsPool();

        protected ReportCellsProvider(string title)
        {
            this.Title = title;
        }

        public string Title { get; }

        public abstract Func<TSourceEntity, ReportCell> CellSelector { get; }

        private ICollection<IReportCellProcessor<TSourceEntity>> Processors { get; } = new List<IReportCellProcessor<TSourceEntity>>();

        private ICollection<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        public IReportCellsProvider<TSourceEntity> AddProcessor(IReportCellProcessor<TSourceEntity> processor)
        {
            this.Processors.Add(processor);

            return this;
        }

        public IReportCellsProvider<TSourceEntity> AddProperty(ReportCellProperty property)
        {
            this.Properties.Add(property);

            return this;
        }

        protected ReportCell<TValue> CreateCell(TValue value, TSourceEntity entity)
        {
            // ReportCell<TValue> cell = new ReportCell<TValue>(value);
            ReportCell<TValue> cell = this.pool.GetOrCreate(
                () => new ReportCell<TValue>(value),
                c => c.Value = value);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

            this.pool.Release(cell);

            return cell;
        }

        private void AddProperties(ReportCell<TValue> cell)
        {
            foreach (ReportCellProperty property in this.Properties)
            {
                cell.AddProperty(property);
            }
        }

        private void RunProcessors(ReportCell<TValue> cell, TSourceEntity entity)
        {
            foreach (IReportCellProcessor<TSourceEntity> processor in this.Processors)
            {
                processor.Process(cell, entity);
            }
        }
    }
}
