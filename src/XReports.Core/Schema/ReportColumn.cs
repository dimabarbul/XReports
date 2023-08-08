using XReports.Table;

namespace XReports.Schema
{
    internal class ReportColumn<TSourceEntity> : IReportColumn<TSourceEntity>
    {
        private readonly string title;
        private readonly IReportCellProvider<TSourceEntity> provider;
        private readonly IReportCellProperty[] cellProperties;
        private readonly IReportCellProperty[] headerProperties;
        private readonly IReportCellProcessor<TSourceEntity>[] cellProcessors;
        private readonly IHeaderReportCellProcessor[] headerProcessors;

        private readonly ReportCell headerCell = new ReportCell();

        public ReportColumn(
            string title,
            IReportCellProvider<TSourceEntity> provider,
            IReportCellProperty[] cellProperties,
            IReportCellProperty[] headerProperties,
            IReportCellProcessor<TSourceEntity>[] cellProcessors,
            IHeaderReportCellProcessor[] headerProcessors)
        {
            this.title = title;
            this.provider = provider;
            this.cellProperties = cellProperties;
            this.headerProperties = headerProperties;
            this.cellProcessors = cellProcessors;
            this.headerProcessors = headerProcessors;
        }

        public ReportCell CreateCell(TSourceEntity item)
        {
            ReportCell cell = this.provider.GetCell(item);

            this.AddProperties(cell);
            this.RunProcessors(cell, item);

            return cell;
        }

        public ReportCell CreateHeaderCell()
        {
            this.headerCell.Clear();
            this.headerCell.SetValue(this.title);

            this.AddHeaderProperties(this.headerCell);
            this.RunHeaderProcessors(this.headerCell);

            return this.headerCell;
        }

        private void AddProperties(ReportCell cell)
        {
            if (this.cellProperties.Length > 0)
            {
                cell.AddProperties(this.cellProperties);
            }
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
            if (this.headerProperties.Length > 0)
            {
                cell.AddProperties(this.headerProperties);
            }
        }

        private void RunHeaderProcessors(ReportCell cell)
        {
            for (int i = 0; i < this.headerProcessors.Length; i++)
            {
                this.headerProcessors[i].Process(cell);
            }
        }
    }
}
