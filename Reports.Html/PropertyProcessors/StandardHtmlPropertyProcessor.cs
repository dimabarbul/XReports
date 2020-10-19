using System;
using System.Collections.Generic;
using System.Linq;
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
            List<(IReportCellProperty Property, IHtmlPropertyHandler Handler)> handlers =
                new List<(IReportCellProperty Property, IHtmlPropertyHandler Handler)>();

            foreach (IReportCellProperty property in cell.Properties)
            {
                IHtmlPropertyHandler handler = this.propertyHandlerFactory(property.GetType());
                if (handler != null)
                {
                    handlers.Add((property, handler));
                }
            }

            foreach ((IReportCellProperty property, IHtmlPropertyHandler handler) in handlers.OrderBy(h => h.Handler.Priority))
            {
                handler?.Handle(property, htmlCell);
            }
        }
    }
}
