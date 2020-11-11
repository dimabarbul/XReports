using Reports.Interfaces;

namespace Reports.PropertyHandlers
{
    public abstract class SingleTypePropertyHandler<TPropertyType, TReportCell> : IPropertyHandler<TReportCell>
        where TPropertyType : IReportCellProperty
    {
        public virtual int Priority => 0;

        public void Handle(IReportCellProperty property, TReportCell cell)
        {
            if (!(property is TPropertyType))
            {
                return;
            }

            this.HandleProperty((TPropertyType) property, cell);
        }

        protected abstract void HandleProperty(TPropertyType property, TReportCell cell);
    }
}
