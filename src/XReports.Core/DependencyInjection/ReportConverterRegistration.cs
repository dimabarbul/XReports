using System;
using XReports.Converter;
using XReports.Table;

namespace XReports.DependencyInjection
{
    internal class ReportConverterRegistration<TReportCell>
        where TReportCell : ReportCell, new()
    {
        public ReportConverterRegistration(string name, Action<TypesCollection<IPropertyHandler<TReportCell>>> configureOptions)
        {
            this.Name = name;
            this.ConfigureOptions = configureOptions;
        }

        public string Name { get; }

        public Action<TypesCollection<IPropertyHandler<TReportCell>>> ConfigureOptions { get; }
    }
}
