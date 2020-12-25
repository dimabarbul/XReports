using Reports.Interfaces;
using Reports.Models;

namespace Reports.PropertyHandlers
{
    public abstract class PropertyHandler<TPropertyType, TReportCell> : IPropertyHandler<TReportCell>
        where TPropertyType : ReportCellProperty
    {
        public virtual int Priority => 0;

        public void Handle(ReportCellProperty property, TReportCell cell)
        {
            if (!(property is TPropertyType))
            {
                return;
            }

            this.HandleProperty((TPropertyType) property, cell);
            property.Processed = true;
        }

        protected abstract void HandleProperty(TPropertyType property, TReportCell cell);
    }
}
