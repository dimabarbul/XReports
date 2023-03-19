using System.Collections.Generic;
using XReports.Table;

namespace XReports.DependencyInjection
{
    internal class ReportConverterFactoryOptions<TReportCell>
        where TReportCell : ReportCell, new()
    {
        public ICollection<ReportConverterRegistration<TReportCell>> Registrations { get; } = new List<ReportConverterRegistration<TReportCell>>();
    }
}
