using XReports.Table;

namespace XReports.Converter
{
    /// <summary>
    /// Base class for property handler that handles only one type of properties (including derived types) during report conversion.
    /// </summary>
    /// <typeparam name="TProperty">Type of property to handle (including derived types).</typeparam>
    /// <typeparam name="TReportCell">Type of cell to handle.</typeparam>
    public abstract class PropertyHandler<TProperty, TReportCell> : IPropertyHandler<TReportCell>
        where TProperty : IReportCellProperty
        where TReportCell : ReportCell
    {
        /// <inheritdoc />
        public virtual int Priority => 0;

        /// <inheritdoc />
        public bool Handle(IReportCellProperty property, TReportCell cell)
        {
            if (property is TProperty typedProperty)
            {
                this.HandleProperty(typedProperty, cell);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles cell property. Will be called for each property of type <typeparamref name="TProperty"/> or derived type for each cell.
        /// </summary>
        /// <param name="property">Property to handle.</param>
        /// <param name="cell">Cell with the property.</param>
        protected abstract void HandleProperty(TProperty property, TReportCell cell);
    }
}
