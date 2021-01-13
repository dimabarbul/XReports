using System.Collections.Generic;
using XReports.Models;

namespace XReports.DependencyInjection
{
    public class ReportConverterFactoryOptions<TReportCell>
        where TReportCell : BaseReportCell, new()
    {
        public ICollection<ReportConverterRegistration<TReportCell>> Registrations { get; } = new List<ReportConverterRegistration<TReportCell>>();
    }
}
