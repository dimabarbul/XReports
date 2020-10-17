using System;
using Reports.Interfaces;
using Reports.Models.Cells;

namespace Reports.Models.Columns
{
    public abstract class ReportColumn<TSourceEntity, TValue> : IReportColumn<TSourceEntity, TValue>
    {
        public string Title { get; }

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

            return cell;
        }

        public abstract Func<TSourceEntity, IReportCell> CellSelector { get; }
    }
}
