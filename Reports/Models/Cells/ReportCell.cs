using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public class ReportCell<TValue> : IReportCell
    {
        public Type ValueType => typeof(TValue);
        public int ColumnSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;

        public IEnumerable<IReportCellProperty> Properties => this.properties;

        protected readonly TValue Value;
        protected IValueFormatter<TValue> Formatter;

        private readonly List<IReportCellProperty> properties;

        public ReportCell(TValue value)
            :this(value, new List<IReportCellProperty>())
        {
        }

        public ReportCell(TValue value, IEnumerable<IReportCellProperty> reportCellProperties)
        {
            this.Value = value;
            this.properties = reportCellProperties.ToList();
        }

        public void SetFormatter(IValueFormatter<TValue> formatter)
        {
            this.Formatter = formatter;
        }

        public virtual string DisplayValue =>
            this.Formatter != null
                ? this.Formatter.Format(this.Value)
                : this.Value?.ToString() ?? string.Empty;


        public bool HasProperty<TProperty>() where TProperty : IReportCellProperty
        {
            return this.Properties.OfType<TProperty>().Any();
        }

        public TProperty GetProperty<TProperty>() where TProperty : IReportCellProperty
        {
            return this.Properties.OfType<TProperty>().FirstOrDefault();
        }

        public void AddProperty<TProperty>(TProperty property) where TProperty : IReportCellProperty
        {
            this.properties.Add(property);
        }
    }
}
