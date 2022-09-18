using System;
using XReports.Models;
using XReports.Options;

namespace XReports.DependencyInjection
{
    internal class ReportConverterRegistration<TReportCell>
        where TReportCell : BaseReportCell, new()
    {
        public string Name { get; set; }

        public Action<ReportConverterOptions<TReportCell>> ConfigureOptions { get; set; }
    }
}
