using System;
using System.Collections.Generic;
using System.Globalization;

namespace XReports.Table
{
    public class ReportCell
    {
        private object value;
        private List<ReportCellProperty> properties = new List<ReportCellProperty>();

        public int ColumnSpan { get; set; } = 1;

        public int RowSpan { get; set; } = 1;

        public Type ValueType { get; private set; } = typeof(string);

        public IReadOnlyList<ReportCellProperty> Properties => this.properties;

        public static ReportCell FromValue<TValue>(TValue value)
        {
            ReportCell cell = new ReportCell();
            cell.SetValue(value);

            return cell;
        }

        public void CopyFrom(ReportCell reportCell)
        {
            this.Clear();
            this.ColumnSpan = reportCell.ColumnSpan;
            this.RowSpan = reportCell.RowSpan;
            this.ValueType = reportCell.ValueType;
            this.value = reportCell.value;
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

            return (TValue)Convert.ChangeType(this.value, typeof(TValue), CultureInfo.CurrentCulture);
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
                if (this.Properties[i].GetType() == typeof(TProperty))
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
                if (this.Properties[i].GetType() == typeof(TProperty))
                {
                    return (TProperty)this.Properties[i];
                }
            }

            return null;
        }

        public void AddProperty(ReportCellProperty property)
        {
            this.properties.Add(property);
        }

        public void AddProperties(IEnumerable<ReportCellProperty> properties)
        {
            this.properties.AddRange(properties);
        }

        public virtual void Clear()
        {
            this.value = string.Empty;
            this.ValueType = typeof(string);
            this.RowSpan = 1;
            this.ColumnSpan = 1;
            this.properties.Clear();
        }

        public virtual ReportCell Clone()
        {
            ReportCell reportCell = (ReportCell)this.MemberwiseClone();
            reportCell.properties = new List<ReportCellProperty>(this.properties);

            return reportCell;
        }
    }
}