using XReports.Interfaces;
using XReports.Models;

namespace XReports.PropertyHandlers
{
    public abstract class PropertyHandler<TPropertyType, TReportCell> : IPropertyHandler<TReportCell>
        where TPropertyType : ReportCellProperty
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
