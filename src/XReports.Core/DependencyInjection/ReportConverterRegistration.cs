using System;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.DependencyInjection
{
    internal class ReportConverterRegistration<TReportCell>
        where TReportCell : BaseReportCell, new()
    {
        public string Name { get; set; }

        public Action<TypesCollection<IPropertyHandler<TReportCell>>> ConfigureOptions { get; set; }
    }
}
