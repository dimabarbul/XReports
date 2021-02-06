using System;
using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.DependencyInjection
{
    public class ReportConverterRegistration<TReportCell>
        where TReportCell : BaseReportCell, new()
    {
        public string Name { get; set; }

        public Type PropertyHandlersInterface { get; set; }

        public IEnumerable<IPropertyHandler<TReportCell>> Handlers { get; set; }
    }
}
