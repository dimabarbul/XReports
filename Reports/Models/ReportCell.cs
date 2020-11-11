using System;
using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;

namespace Reports.Models
{
    public abstract class BaseReportCell
    {
        public int ColumnSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
        public Type ValueType { get; set; } = typeof(string);

        public virtual dynamic InternalValue { get; set; }

        public virtual void CopyFrom(BaseReportCell reportCell)
        {
            this.ColumnSpan = reportCell.ColumnSpan;
            this.RowSpan = reportCell.RowSpan;
            this.ValueType = reportCell.ValueType;
            this.InternalValue = reportCell.InternalValue;
        }
    }

    public abstract class ReportCell : BaseReportCell
    {
        public List<IReportCellProperty> Properties { get; } = new List<IReportCellProperty>();

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
            this.Properties.Add(property);
        }

        public TValue GetValue<TValue>()
        {
            if (this.ValueType != typeof(TValue))
            {
                throw new InvalidCastException($"Value type is {this.ValueType} but requested {typeof(TValue)}");
            }

            return (TValue) this.InternalValue;
        }
    }

    public class ReportCell<TValue> : ReportCell
    {
        private TValue value;

        public ReportCell(TValue value)
        {
            this.value = value;
            this.ValueType = typeof(TValue);
        }

        public override dynamic InternalValue
        {
            get => this.value;
            set => this.value = value;
        }
    }
}
