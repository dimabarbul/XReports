using System.Collections.Generic;
using XReports.Interfaces;
using XReports.ReportSchemaCellsProviders;

namespace XReports.Models
{
    public abstract partial class ReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        protected IReadOnlyList<ReportSchemaCellsProvider<TSourceEntity>> CellsProviders { get; }
        protected IReadOnlyList<ReportTableProperty> TableProperties { get; }

#pragma warning disable CA1819 // OK for complex header
        protected ComplexHeaderCell[,] ComplexHeader { get; }
#pragma warning restore CA1819

        protected IReadOnlyDictionary<string, ReportCellProperty[]> ComplexHeaderProperties { get; }
        protected IReadOnlyList<ReportCellProperty> CommonComplexHeaderProperties { get; }

        protected ReportSchema(ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders, ReportTableProperty[] tableProperties, ComplexHeaderCell[,] complexHeader, Dictionary<string, ReportCellProperty[]> complexHeaderProperties, ReportCellProperty[] commonComplexHeaderProperties)
        {
            this.CellsProviders = cellsProviders;
            this.TableProperties = tableProperties;
            this.ComplexHeader = complexHeader;
            this.ComplexHeaderProperties = complexHeaderProperties;
            this.CommonComplexHeaderProperties = commonComplexHeaderProperties;
        }

        public abstract IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
