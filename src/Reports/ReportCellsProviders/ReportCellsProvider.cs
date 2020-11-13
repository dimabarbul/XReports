using System;
using System.Collections.Generic;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    public abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity, TValue>
    {
        public string Title { get; }
        public ICollection<IReportCellProcessor> Processors { get; } = new List<IReportCellProcessor>();
        public ICollection<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        protected ReportCellsProvider(string title)
        {
            this.Title = title;
        }

        public void AddProcessor(IReportCellProcessor processor)
        {
            this.Processors.Add(processor);
        }

        public void AddProperty(ReportCellProperty property)
        {
            this.Properties.Add(property);
        }

        protected ReportCell<TValue> CreateCell(TValue value)
        {
            ReportCell<TValue> cell = new ReportCell<TValue>(value);

            this.AddProperties(cell);
            this.RunProcessors(cell);

            return cell;
        }

        private void AddProperties(ReportCell<TValue> cell)
        {
            foreach (ReportCellProperty property in this.Properties)
            {
                cell.AddProperty(property);
            }
        }

        private void RunProcessors(ReportCell<TValue> cell)
        {
            foreach (IReportCellProcessor processor in this.Processors)
            {
                processor.Process(cell);
            }
        }

        public abstract Func<TSourceEntity, ReportCell> CellSelector { get; }
    }
}
