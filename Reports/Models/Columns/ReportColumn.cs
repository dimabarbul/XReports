using System;
using Reports.Factories;
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
            IReportCell cell = ReportCellFactory.Create<TValue>(value);

            if (this.formatter != null)
            {
                (cell as ReportCell<TValue>)?.SetFormatter(this.formatter);
            }

            return cell;
        }

        public abstract Func<TSourceEntity, IReportCell> CellSelector { get; }
    }
}
