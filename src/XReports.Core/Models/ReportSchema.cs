using System.Collections.Generic;
using XReports.Interfaces;
using XReports.ReportCellsProviders;

namespace XReports.Models
{
    public abstract partial class ReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        protected ReportSchemaCellsProvider<TSourceEntity>[] CellsProviders { get; private set; }

        protected ReportTableProperty[] TableProperties { get; private set; }

        protected ComplexHeader[] ComplexHeaders { get; private set; }

        protected Dictionary<string, ReportCellProperty[]> ComplexHeaderProperties { get; private set; }

        protected ReportCellProperty[] CommonComplexHeaderProperties { get; private set; }

        public static VerticalReportSchema<TSourceEntity> CreateVertical(
            ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders,
            ReportTableProperty[] tableProperties,
            ComplexHeader[] complexHeaders,
            Dictionary<string, ReportCellProperty[]> complexHeaderProperties,
            ReportCellProperty[] commonComplexHeaderProperties)
        {
            return new VerticalReportSchema<TSourceEntity>()
            {
                CellsProviders = cellsProviders,
                TableProperties = tableProperties,
                ComplexHeaders = complexHeaders,
                ComplexHeaderProperties = complexHeaderProperties,
                CommonComplexHeaderProperties = commonComplexHeaderProperties,
            };
        }

        public static HorizontalReportSchema<TSourceEntity> CreateHorizontal(
            ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders,
            ReportTableProperty[] tableProperties,
            ComplexHeader[] complexHeaders,
            Dictionary<string, ReportCellProperty[]> complexHeaderProperties,
            ReportSchemaCellsProvider<TSourceEntity>[] headerRows,
            ReportCellProperty[] commonComplexHeaderProperties)
        {
            return new HorizontalReportSchema<TSourceEntity>()
            {
                CellsProviders = cellsProviders,
                TableProperties = tableProperties,
                ComplexHeaders = complexHeaders,
                ComplexHeaderProperties = complexHeaderProperties,
                HeaderRows = headerRows,
                CommonComplexHeaderProperties = commonComplexHeaderProperties,
            };
        }

        public abstract IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
