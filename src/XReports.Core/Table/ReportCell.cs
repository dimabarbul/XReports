using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace XReports.Table
{
    /// <summary>
    /// Report cell.
    /// </summary>
    public class ReportCell
    {
        private object value;
        private List<ReportCellProperty> properties = new List<ReportCellProperty>();

        /// <summary>
        /// Gets or sets how many columns the cell spans.
        /// </summary>
        public int ColumnSpan { get; set; } = 1;

        /// <summary>
        /// Gets or sets how many rows the cell spans.
        /// </summary>
        public int RowSpan { get; set; } = 1;

        /// <summary>
        /// Gets type of cell value.
        /// </summary>
        public Type ValueType { get; private set; } = typeof(string);

        /// <summary>
        /// Gets cell properties.
        /// </summary>
        public IReadOnlyList<ReportCellProperty> Properties => this.properties;

        /// <summary>
        /// Copies value from another cell along with value type.
        /// </summary>
        /// <param name="reportCell">Cell to copy value from.</param>
        public void CopyValueFrom(ReportCell reportCell)
        {
            Debug.Assert(reportCell != null, "Cannot copy value from null cell");

            this.ValueType = reportCell.ValueType;
            this.value = reportCell.value;
        }

        /// <summary>
        /// Returns underlying value of the cell, i.e., without any conversion or casting.
        /// </summary>
        /// <returns>Cell underlying value.</returns>
        public object GetUnderlyingValue()
        {
            return this.value;
        }

        /// <summary>
        /// Returns cell value converted to type <typeparamref name="TValue"/>.
        /// </summary>
        /// <remarks>If <typeparamref name="TValue"/> is a value type and cell value is null, exception will be thrown. Use <see cref="GetNullableValue{TValue}"/> instead in this case.</remarks>
        /// <typeparam name="TValue">Type to convert value to.</typeparam>
        /// <returns>Cell value converted to type <typeparamref name="TValue"/>.</returns>
        public TValue GetValue<TValue>()
        {
            if (this.ValueType == typeof(TValue))
            {
                return (TValue)this.value;
            }

            return (TValue)Convert.ChangeType(this.value, typeof(TValue), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Sets cell value.
        /// </summary>
        /// <param name="value">New cell value.</param>
        /// <typeparam name="TValue">Type of new cell value.</typeparam>
        public void SetValue<TValue>(TValue value)
        {
            this.ValueType = typeof(TValue);
            this.value = value;
        }

        /// <summary>
        /// Returns cell value converted to value type <typeparamref name="TValue"/> or null is cell value is null.
        /// </summary>
        /// <typeparam name="TValue">Value type to convert value to.</typeparam>
        /// <returns>Cell value converted to value type <typeparamref name="TValue"/> or null.</returns>
        public TValue? GetNullableValue<TValue>()
            where TValue : struct
        {
            if (this.value == null)
            {
                return null;
            }

            return this.GetValue<TValue>();
        }

        /// <summary>
        /// Checks if cell has property of type <typeparamref name="TProperty"/>. Derived types are ignored.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to check for.</typeparam>
        /// <returns>True if cell has property of type <typeparamref name="TProperty"/>, false otherwise.</returns>
        public bool HasProperty<TProperty>()
            where TProperty : ReportCellProperty
        {
            return this.HasProperty(typeof(TProperty));
        }

        /// <summary>
        /// Checks if cell has property of specified type. Derived types are ignored.
        /// </summary>
        /// <param name="propertyType">Type of property to check for.</param>
        /// <returns>True if cell has property of specified type, false otherwise.</returns>
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

        /// <summary>
        /// Returns first property of type <typeparamref name="TProperty"/> or null if there is no such property. Derived types are ignored.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to get.</typeparam>
        /// <returns>First property of type <typeparamref name="TProperty"/> or null.</returns>
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

        /// <summary>
        /// Adds cell property.
        /// </summary>
        /// <param name="property">Property to add.</param>
        public void AddProperty(ReportCellProperty property)
        {
            this.properties.Add(property);
        }

        /// <summary>
        /// Adds cell properties.
        /// </summary>
        /// <param name="properties">Properties to add.</param>
        public void AddProperties(IEnumerable<ReportCellProperty> properties)
        {
            this.properties.AddRange(properties);
        }

        /// <summary>
        /// Clears cell content and any properties or data it has. After calling this method the cell should be the same as it was when created.
        /// </summary>
        public virtual void Clear()
        {
            this.value = string.Empty;
            this.ValueType = typeof(string);
            this.RowSpan = 1;
            this.ColumnSpan = 1;
            this.properties.Clear();
        }

        /// <summary>
        /// Makes a deep clone of the cell. Any collections should be different, but objects these collections contain may be the same. For example, properties collection in clone will be different, but the properties in it may refer to the same properties as in original cell.
        /// </summary>
        /// <returns>Deep cell clone.</returns>
        public virtual ReportCell Clone()
        {
            ReportCell reportCell = (ReportCell)this.MemberwiseClone();
            reportCell.properties = new List<ReportCellProperty>(this.properties);

            return reportCell;
        }
    }
}
