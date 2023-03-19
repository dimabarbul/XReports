using XReports.Table;

namespace XReports.Converter
{
    public abstract class PropertyHandler<TProperty, TReportCell> : IPropertyHandler<TReportCell>
        where TProperty : ReportCellProperty
        where TReportCell : ReportCell
    {
        public virtual int Priority => 0;

        public bool Handle(ReportCellProperty property, TReportCell cell)
        {
            if (property is TProperty typedProperty)
            {
                this.HandleProperty(typedProperty, cell);

                return true;
            }

            return false;
        }

        protected abstract void HandleProperty(TProperty property, TReportCell cell);
    }
}
