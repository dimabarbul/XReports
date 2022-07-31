using System.Runtime.CompilerServices;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class ReportSchemaCellsProvider<TSourceEntity>
    {
        private readonly IReportCellsProvider<TSourceEntity> provider;
        private readonly ReportCellProperty[] cellProperties;
        private readonly ReportCellProperty[] headerProperties;
        private readonly IReportCellProcessor<TSourceEntity>[] cellProcessors;
        private readonly IReportCellProcessor<TSourceEntity>[] headerProcessors;

        private readonly ReportCell headerCell = new ReportCell();

        public ReportSchemaCellsProvider(
            IReportCellsProvider<TSourceEntity> provider,
            ReportCellProperty[] cellProperties,
            ReportCellProperty[] headerProperties,
            IReportCellProcessor<TSourceEntity>[] cellProcessors,
            IReportCellProcessor<TSourceEntity>[] headerProcessors)
        {
            this.provider = provider;
            this.cellProperties = cellProperties;
            this.headerProperties = headerProperties;
            this.cellProcessors = cellProcessors;
            this.headerProcessors = headerProcessors;
        }

        public ReportCell CreateCell(TSourceEntity entity)
        {
            ReportCell cell = this.provider.GetCell(entity);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

            return cell;
        }

        public ReportCell CreateHeaderCell()
        {
            this.headerCell.Clear();
            this.headerCell.SetValue(this.provider.Title);

            this.AddHeaderProperties(this.headerCell);
            this.RunHeaderProcessors(this.headerCell);

            return this.headerCell;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddProperties(ReportCell cell)
        {
            if (this.cellProperties.Length > 0)
            {
                cell.AddProperties(this.cellProperties);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunProcessors(ReportCell cell, TSourceEntity entity)
        {
            for (int i = 0; i < this.cellProcessors.Length; i++)
            {
                this.cellProcessors[i].Process(cell, entity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddHeaderProperties(ReportCell cell)
        {
            if (this.headerProperties.Length > 0)
            {
                cell.AddProperties(this.headerProperties);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunHeaderProcessors(ReportCell cell)
        {
            for (int i = 0; i < this.headerProcessors.Length; i++)
            {
                this.headerProcessors[i].Process(cell, default);
            }
        }
    }
}
