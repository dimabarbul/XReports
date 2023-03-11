using XReports.Table;

namespace XReports.Converter
{
    public abstract class PropertyHandler<TPropertyType, TReportCell> : IPropertyHandler<TReportCell>
        where TPropertyType : ReportCellProperty
        where TReportCell : ReportCell
    {
        public virtual int Priority => 0;

        public bool Handle(ReportCellProperty property, TReportCell cell)
        {
            if (property is TPropertyType typedProperty)
            {
                this.HandleProperty(typedProperty, cell);

                return true;
            }

            return false;
        }

        protected abstract void HandleProperty(TPropertyType property, TReportCell cell);
    }
}
