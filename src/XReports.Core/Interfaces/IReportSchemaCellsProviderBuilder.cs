using System.Collections.Generic;
using XReports.Models;
using XReports.ReportSchemaCellsProviders;

namespace XReports.Interfaces
{
    public interface IReportSchemaCellsProviderBuilder<TSourceEntity>
    {
        string Title { get; }
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties);
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties);
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors);
        IReportSchemaCellsProviderBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors);
        ReportSchemaCellsProvider<TSourceEntity> Build(IReadOnlyList<ReportCellProperty> globalProperties);
    }
}
