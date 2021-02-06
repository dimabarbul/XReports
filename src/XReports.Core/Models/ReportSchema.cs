using System.Collections.Generic;
using XReports.Interfaces;

namespace XReports.Models
{
    public abstract partial class ReportSchema<TSourceEntity> : IReportSchema<TSourceEntity>
    {
        protected ReportSchemaCellsProvider<TSourceEntity>[] CellsProviders { get; private set; }

        protected ReportCellProperty[] TableProperties { get; private set; }

        protected ComplexHeader[] ComplexHeaders { get; private set; }

        protected Dictionary<string, ReportCellProperty[]> ComplexHeaderProperties { get; private set; }

        protected ReportCellProperty[] CommonComplexHeaderProperties { get; private set; }

        public static VerticalReportSchema<TSourceEntity> CreateVertical(
            ReportSchemaCellsProvider<TSourceEntity>[] cellsProviders,
            ReportCellProperty[] tableProperties,
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
            ReportCellProperty[] tableProperties,
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

        protected ReportCell AddTableProperties(ReportCell cell)
        {
            foreach (ReportCellProperty tableProperty in this.TableProperties)
            {
                if (!cell.HasProperty(tableProperty.GetType()))
                {
                    cell.AddProperty(tableProperty);
                }
            }

            return cell;
        }
    }
}
