using System;
using XReports.Converter;
using XReports.Table;

namespace XReports.DependencyInjection
{
    internal class ReportConverterRegistration<TReportCell>
        where TReportCell : ReportCell, new()
    {
        public string Name { get; set; }

        public Action<TypesCollection<IPropertyHandler<TReportCell>>> ConfigureOptions { get; set; }
    }
}
