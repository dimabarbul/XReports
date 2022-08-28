using System.Collections.Generic;
using XReports.Interfaces;
using XReports.ReportCellsProviders;

namespace XReports.Models
{
    public abstract partial class ReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        protected IReadOnlyList<ReportSchemaCellsProvider<TSourceEntity>> CellsProviders { get; }
        protected IReadOnlyList<ReportTableProperty> TableProperties { get; }
        protected IReadOnlyList<ComplexHeader> ComplexHeaders { get; }
        protected IReadOnlyDictionary<string, ReportCellProperty[]> ComplexHeaderProperties { get; }
        protected IReadOnlyList<ReportCellProperty> CommonComplexHeaderProperties { get; }

        protected ReportSchema(ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders, ReportTableProperty[] tableProperties, ComplexHeader[] complexHeaders, Dictionary<string, ReportCellProperty[]> complexHeaderProperties, ReportCellProperty[] commonComplexHeaderProperties)
        {
            this.CellsProviders = cellsProviders;
            this.TableProperties = tableProperties;
            this.ComplexHeaders = complexHeaders;
            this.ComplexHeaderProperties = complexHeaderProperties;
            this.CommonComplexHeaderProperties = commonComplexHeaderProperties;
        }

        public abstract IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
