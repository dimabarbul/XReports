using System;
using System.Collections.Generic;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    internal abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity, TValue>
    {
        public string Title { get; }
        public ICollection<IReportCellProcessor> Processors { get; } = new List<IReportCellProcessor>();
        public ICollection<IReportCellProperty> Properties { get; } = new List<IReportCellProperty>();

        private IValueFormatter<TValue> formatter;

        protected ReportCellsProvider(string title)
        {
            this.Title = title;
        }

        public void SetValueFormatter(IValueFormatter<TValue> valueFormatter)
        {
            this.formatter = valueFormatter;
        }

        public void AddProcessor(IReportCellProcessor processor)
        {
            this.Processors.Add(processor);
        }

        public void AddProperty(IReportCellProperty property)
        {
            this.Properties.Add(property);
        }

        protected IReportCell CreateCell(TValue value)
        {
            ReportCell<TValue> cell = new ReportCell<TValue>(value);

            this.FormatCell(cell);
            this.AddProperties(cell);
            this.RunProcessors(cell);

            return cell;
        }

        private void FormatCell(ReportCell<TValue> cell)
        {
            if (this.formatter != null)
            {
                cell.SetFormatter(this.formatter);
            }
        }

        private void AddProperties(ReportCell<TValue> cell)
        {
            foreach (IReportCellProperty property in this.Properties)
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

        public abstract Func<TSourceEntity, IReportCell> CellSelector { get; }
    }
}
