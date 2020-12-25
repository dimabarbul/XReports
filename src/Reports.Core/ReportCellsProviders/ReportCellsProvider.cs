using System;
using System.Collections.Generic;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    public abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity, TValue>
    {
        public string Title { get; }
        public ICollection<IReportCellProcessor<TSourceEntity>> Processors { get; } = new List<IReportCellProcessor<TSourceEntity>>();
        public ICollection<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        protected ReportCellsProvider(string title)
        {
            this.Title = title;
        }

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
            ReportCell<TValue> cell = new ReportCell<TValue>(value);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

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

        public abstract Func<TSourceEntity, ReportCell> CellSelector { get; }
    }
}
