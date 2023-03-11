using System.Collections.Generic;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilder
{
    public interface IReportColumnBuilder<TSourceEntity>
    {
        IReportColumnBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties);
        IReportColumnBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties);
        IReportColumnBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors);
        IReportColumnBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors);
        IReportColumn<TSourceEntity> Build(IReadOnlyList<ReportCellProperty> globalProperties);
    }
}
