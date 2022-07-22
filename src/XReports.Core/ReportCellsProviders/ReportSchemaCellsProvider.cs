using XReports.Interfaces;
using XReports.Models;
using XReports.Utils;

namespace XReports.ReportCellsProviders
{
    public class ReportSchemaCellsProvider<TSourceEntity>
    {
        private readonly IReportCellsProvider<TSourceEntity> provider;
        private readonly ReportCellProperty[] cellProperties;
        private readonly ReportCellProperty[] headerProperties;
        private readonly IReportCellProcessor<TSourceEntity>[] cellProcessors;
        private readonly IReportCellProcessor<TSourceEntity>[] headerProcessors;
        private readonly ReportCellsPool<ReportCell> pool = new ReportCellsPool<ReportCell>();

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
            ReportCell cell = ReportCell.FromValue(this.provider.Title);
            ////ReportCell cell = this.pool.GetOrCreate(
            ////    () => new ReportCell { Value = this.provider.Title },
            ////    c => c.Value = this.provider.Title);

            this.AddHeaderProperties(cell);
            this.RunHeaderProcessors(cell);

            //// this.pool.Release(cell);

            return cell;
        }

        private void AddProperties(ReportCell cell)
        {
            cell.AddProperties(this.cellProperties);
        }

        private void RunProcessors(ReportCell cell, TSourceEntity entity)
        {
            for (int i = 0; i < this.cellProcessors.Length; i++)
            {
                this.cellProcessors[i].Process(cell, entity);
            }
        }

        private void AddHeaderProperties(ReportCell cell)
        {
            cell.AddProperties(this.headerProperties);
        }

        private void RunHeaderProcessors(ReportCell cell)
        {
            for (int i = 0; i < this.headerProcessors.Length; i++)
            {
                this.headerProcessors[i].Process(cell, default);
            }
        }
    }
}
