using System.Collections.Generic;
using XReports.Table;

namespace XReports.Schema
{
    internal abstract partial class ReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        private readonly ComplexHeaderCell[,] complexHeader;
        private readonly IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties;
        private readonly IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties;

        protected IReadOnlyList<IReportColumn<TSourceEntity>> Columns { get; }
        protected IReadOnlyList<ReportTableProperty> TableProperties { get; }

        protected ReportSchema(
            IReadOnlyList<IReportColumn<TSourceEntity>> columns,
            IReadOnlyList<ReportTableProperty> tableProperties,
            ComplexHeaderCell[,] complexHeader,
            IReadOnlyDictionary<string, ReportCellProperty[]> complexHeaderProperties,
            IReadOnlyList<ReportCellProperty> commonComplexHeaderProperties)
        {
            this.Columns = columns;
            this.TableProperties = tableProperties;
            this.complexHeader = complexHeader;
            this.complexHeaderProperties = complexHeaderProperties;
            this.commonComplexHeaderProperties = commonComplexHeaderProperties;
        }

        public abstract IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
