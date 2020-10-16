using System;
using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public abstract class ReportCell<TValue> : IReportCell
    {
        public Type ValueType => typeof(TValue);

        protected TValue Value;

        public void SetValue(TValue value)
        {
            this.Value = value;
        }

        public abstract string DisplayValue { get; }
        public abstract void SetValueFormatOptions(object formatOptions);
    }
}
