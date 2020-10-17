using System;
using System.Collections.Generic;
using Reports.Interfaces;
using Reports.Models.Cells;

namespace Reports.Models.Columns
{
    public abstract class ReportColumn<TSourceEntity, TValue> : IReportColumn<TSourceEntity, TValue>
    {
        public string Title { get; }
        public ICollection<IReportCellProcessor> Processors { get; } = new List<IReportCellProcessor>();

        private IValueFormatter<TValue> formatter;

        protected ReportColumn(string title)
        {
            this.Title = title;
        }

        public void SetValueFormatter(IValueFormatter<TValue> valueFormatter)
        {
            this.formatter = valueFormatter;
        }

        protected IReportCell CreateCell(TValue value)
        {
            ReportCell<TValue> cell = new ReportCell<TValue>(value);

            if (this.formatter != null)
            {
                cell.SetFormatter(this.formatter);
            }

            foreach (IReportCellProcessor processor in this.Processors)
            {
                processor.Process(cell);
            }

            return cell;
        }

        public abstract Func<TSourceEntity, IReportCell> CellSelector { get; }
    }
}
