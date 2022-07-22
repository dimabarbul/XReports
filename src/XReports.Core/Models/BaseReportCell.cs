using System;
using System.Collections.Generic;

namespace XReports.Models
{
    public abstract class BaseReportCell
    {
        private object value;

        public int ColumnSpan { get; set; } = 1;

        public int RowSpan { get; set; } = 1;

        public Type ValueType { get; private set; } = typeof(string);

        public List<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        public virtual void CopyFrom(BaseReportCell reportCell)
        {
            this.ColumnSpan = reportCell.ColumnSpan;
            this.RowSpan = reportCell.RowSpan;
            this.ValueType = reportCell.ValueType;
            this.value = reportCell.value;
            this.Properties.Clear();
        }

        public object GetUnderlyingValue()
        {
            return this.value;
        }

        public TValue GetValue<TValue>()
        {
            if (this.ValueType == typeof(TValue))
            {
                return (TValue)this.value;
            }

            return (TValue)Convert.ChangeType(this.value, typeof(TValue));
        }

        public void SetValue<TValue>(TValue value)
        {
            this.ValueType = typeof(TValue);
            this.value = value;
        }

        public TValue? GetNullableValue<TValue>()
            where TValue : struct
        {
            if (this.value == null)
            {
                return null;
            }

            return this.GetValue<TValue>();
        }

        public bool HasProperty<TProperty>()
            where TProperty : ReportCellProperty
        {
            for (int i = 0; i < this.Properties.Count; i++)
            {
                if (this.Properties[i] is TProperty)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasProperty(Type propertyType)
        {
            for (int i = 0; i < this.Properties.Count; i++)
            {
                if (this.Properties[i].GetType() == propertyType)
                {
                    return true;
                }
            }

            return false;
        }

        public TProperty GetProperty<TProperty>()
            where TProperty : ReportCellProperty
        {
            for (int i = 0; i < this.Properties.Count; i++)
            {
                if (this.Properties[i] is TProperty cellProperty)
                {
                    return cellProperty;
                }
            }

            return null;
        }

        public void AddProperty(ReportCellProperty property)
        {
            this.Properties.Add(property);
        }

        public void AddProperties(IEnumerable<ReportCellProperty> properties)
        {
            this.Properties.AddRange(properties);
        }

        public void Clear()
        {
            this.value = string.Empty;
            this.ValueType = typeof(string);
            this.RowSpan = 1;
            this.ColumnSpan = 1;
            this.Properties.Clear();
        }
    }
}
