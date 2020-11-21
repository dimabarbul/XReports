using System.Collections.Generic;
using Reports.Interfaces;

namespace Reports.Models
{
    public abstract class ReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        protected ReportSchemaCellsProvider<TSourceEntity>[] CellsProviders { get; private set; }
        protected ReportCellProperty[] TableProperties { get; private set; }

        public static VerticalReportSchema<TSourceEntity> CreateVertical(
            ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders,
            ReportCellProperty[] tableProperties,
            ComplexHeader[] complexHeaders,
            Dictionary<string, ReportCellProperty[]> complexHeaderProperties
        )
        {
            return new VerticalReportSchema<TSourceEntity>()
            {
                CellsProviders = cellsProviders,
                TableProperties = tableProperties,
                ComplexHeaders = complexHeaders,
                ComplexHeaderProperties = complexHeaderProperties,
            };
        }

        public static HorizontalReportSchema<TSourceEntity> CreateHorizontal(
            ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders,
            ReportCellProperty[] tableProperties,
            ReportSchemaCellsProvider<TSourceEntity>[] headerRows
        )
        {
            return new HorizontalReportSchema<TSourceEntity>()
            {
                CellsProviders = cellsProviders,
                TableProperties = tableProperties,
                HeaderRows = headerRows,
            };
        }

        public abstract IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
