using System;
using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public abstract class ReportCell<TValue> : IReportCell
    {
        public Type ValueType => typeof(TValue);

        protected TValue Value;
        protected IValueFormatter<TValue> Formatter;

        public void SetValue(TValue value)
        {
            this.Value = value;
        }

        public void SetFormatter(IValueFormatter<TValue> formatter)
        {
            this.Formatter = formatter;
        }

        public virtual string DisplayValue =>
            this.Formatter != null ?
                this.Formatter.Format(this.Value) :
                this.Value?.ToString() ?? string.Empty;
    }
}
