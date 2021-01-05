using System;
using System.Collections.Generic;
using System.Linq;

namespace XReports.Models
{
    public abstract class BaseReportCell
    {
        private dynamic value;

        public int ColumnSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
        public Type ValueType { get; set; } = typeof(string);
        public List<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        public dynamic Value
        {
            get => this.value;
            set
            {
                this.value = value;

                if (value != null)
                {
                    this.ValueType = value.GetType();
                }
                else
                {
                    bool isCurrentTypeAllowsNull = !this.ValueType.IsValueType || (Nullable.GetUnderlyingType(this.ValueType) != null);
                    if (!isCurrentTypeAllowsNull)
                    {
                        this.ValueType = typeof(string);
                    }
                }
            }
        }

        public virtual void CopyFrom(BaseReportCell reportCell)
        {
            this.ColumnSpan = reportCell.ColumnSpan;
            this.RowSpan = reportCell.RowSpan;
            this.ValueType = reportCell.ValueType;
            this.value = reportCell.Value;
        }

        public TValue GetValue<TValue>()
        {
            if (this.ValueType == typeof(TValue))
            {
                return this.value;
            }

            return Convert.ChangeType(this.value, typeof(TValue));
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
