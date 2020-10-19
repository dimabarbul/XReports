using System;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Interfaces;

namespace Reports.Html.PropertyHandlers
{
    public abstract class HtmlPropertyHandle<TPropertyType> : IHtmlPropertyHandler
        where TPropertyType : IReportCellProperty
    {
        public void Handle(IReportCellProperty property, HtmlReportTableCell cell)
        {
            if (!(property is TPropertyType))
            {
                throw new ArgumentOutOfRangeException();
            }

            this.HandleProperty((TPropertyType) property, cell);
        }

        protected abstract void HandleProperty(TPropertyType property, HtmlReportTableCell cell);
    }
}
