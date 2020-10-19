using System;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Interfaces;

namespace Reports.Html.PropertyProcessors
{
    public class StandardHtmlPropertyProcessor : IHtmlPropertyProcessor
    {
        private readonly Func<Type, IHtmlPropertyHandler> propertyHandlerFactory;

        public StandardHtmlPropertyProcessor(Func<Type, IHtmlPropertyHandler> propertyHandlerFactory)
        {
            this.propertyHandlerFactory = propertyHandlerFactory;
        }

        public void ProcessProperties(IReportCell cell, HtmlReportTableCell htmlCell)
        {
            foreach (IReportCellProperty property in cell.Properties)
            {
                IHtmlPropertyHandler handler = this.propertyHandlerFactory(property.GetType());
                handler?.Handle(property, htmlCell);
            }
        }
    }
}
