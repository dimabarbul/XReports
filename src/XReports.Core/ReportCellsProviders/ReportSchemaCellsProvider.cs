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
        private readonly ReportCellsPool pool = new ReportCellsPool();

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
            ReportCell cell = this.provider.CellSelector(entity);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

            return cell;
        }

        public ReportCell CreateHeaderCell()
        {
            // ReportCell cell = new ReportCell<string>(this.provider.Title);
            ReportCell cell = this.pool.GetOrCreate(
                () => new ReportCell<string>(this.provider.Title),
                c => c.Value = this.provider.Title);

            this.AddHeaderProperties(cell);
            this.RunHeaderProcessors(cell);

            this.pool.Release(cell);

            return cell;
        }

        private void AddProperties(ReportCell cell)
        {
            foreach (ReportCellProperty property in this.cellProperties)
            {
                cell.AddProperty(property);
            }
        }

        private void RunProcessors(ReportCell cell, TSourceEntity entity)
        {
            foreach (IReportCellProcessor<TSourceEntity> processor in this.cellProcessors)
            {
                processor.Process(cell, entity);
            }
        }

        private void AddHeaderProperties(ReportCell cell)
        {
            foreach (ReportCellProperty property in this.headerProperties)
            {
                cell.AddProperty(property);
            }
        }

        private void RunHeaderProcessors(ReportCell cell)
        {
            foreach (IReportCellProcessor<TSourceEntity> processor in this.headerProcessors)
            {
                processor.Process(cell, default);
            }
        }
    }
}
