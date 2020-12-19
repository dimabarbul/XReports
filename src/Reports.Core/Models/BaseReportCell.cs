using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports.Core.Models
{
    public abstract class BaseReportCell
    {
        public int ColumnSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
        public Type ValueType { get; set; } = typeof(string);
        public List<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        public virtual dynamic InternalValue { get; set; }

        public virtual void CopyFrom(BaseReportCell reportCell)
        {
            this.ColumnSpan = reportCell.ColumnSpan;
            this.RowSpan = reportCell.RowSpan;
            this.ValueType = reportCell.ValueType;
            this.InternalValue = reportCell.InternalValue;
        }

        public TValue GetValue<TValue>()
        {
            if (this.ValueType == typeof(TValue))
            {
                return this.InternalValue;
            }

            return Convert.ChangeType(this.InternalValue, typeof(TValue));
        }

        public TValue? GetNullableValue<TValue>()
            where TValue : struct
        {
            if (this.InternalValue == null)
            {
                return null;
            }

            return this.GetValue<TValue>();
        }

        public bool HasProperty<TProperty>() where TProperty : ReportCellProperty
        {
            return this.Properties.OfType<TProperty>().Any();
        }

        public bool HasProperty(Type propertyType)
        {
            return this.Properties.Any(p => p.GetType() == propertyType);
        }

        public TProperty GetProperty<TProperty>() where TProperty : ReportCellProperty
        {
            return this.Properties.OfType<TProperty>().FirstOrDefault();
        }

        public void AddProperty<TProperty>(TProperty property) where TProperty : ReportCellProperty
        {
            this.Properties.Add(property);
        }
    }
}
