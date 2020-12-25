using XReports.Interfaces;

namespace XReports.Models
{
    public class ReportSchemaCellsProvider<TSourceEntity>
    {
        public readonly IReportCellsProvider<TSourceEntity> Provider;
        public readonly ReportCellProperty[] CellProperties;
        public readonly ReportCellProperty[] HeaderProperties;
        public readonly IReportCellProcessor<TSourceEntity>[] CellProcessors;
        public readonly IReportCellProcessor<TSourceEntity>[] HeaderProcessors;

        public ReportSchemaCellsProvider(IReportCellsProvider<TSourceEntity> provider,
            ReportCellProperty[] cellProperties, ReportCellProperty[] headerProperties,
            IReportCellProcessor<TSourceEntity>[] cellProcessors,
            IReportCellProcessor<TSourceEntity>[] headerProcessors)
        {
            this.Provider = provider;
            this.CellProperties = cellProperties;
            this.HeaderProperties = headerProperties;
            this.CellProcessors = cellProcessors;
            this.HeaderProcessors = headerProcessors;
        }

        public ReportCell CreateCell(TSourceEntity entity)
        {
            ReportCell cell = this.Provider.CellSelector(entity);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

            return cell;
        }

        public ReportCell CreateHeaderCell()
        {
            ReportCell cell = new ReportCell<string>(this.Provider.Title);

            this.AddHeaderProperties(cell);
            this.RunHeaderProcessors(cell);

            return cell;
        }

        private void AddProperties(ReportCell cell)
        {
            foreach (ReportCellProperty property in this.CellProperties)
            {
                cell.AddProperty(property);
            }
        }

        private void RunProcessors(ReportCell cell, TSourceEntity entity)
        {
            foreach (IReportCellProcessor<TSourceEntity> processor in this.CellProcessors)
            {
                processor.Process(cell, entity);
            }
        }

        private void AddHeaderProperties(ReportCell cell)
        {
            foreach (ReportCellProperty property in this.HeaderProperties)
            {
                cell.AddProperty(property);
            }
        }

        private void RunHeaderProcessors(ReportCell cell)
        {
            foreach (IReportCellProcessor<TSourceEntity> processor in this.HeaderProcessors)
            {
                processor.Process(cell, default);
            }
        }
    }
}
