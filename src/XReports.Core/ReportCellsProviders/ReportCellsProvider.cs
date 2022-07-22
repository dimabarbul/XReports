using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
        private readonly ReportCell reportCell = new ReportCell();

        protected ReportCellsProvider(string title)
        {
            this.Title = title;
        }

        public string Title { get; }

        private List<IReportCellProcessor<TSourceEntity>> Processors { get; } = new List<IReportCellProcessor<TSourceEntity>>();

        private ICollection<ReportCellProperty> Properties { get; } = new List<ReportCellProperty>();

        public IReportCellsProvider<TSourceEntity> AddProcessor(IReportCellProcessor<TSourceEntity> processor)
        {
            this.Processors.Add(processor);

            return this;
        }

        public IReportCellsProvider<TSourceEntity> AddProperty(ReportCellProperty property)
        {
            this.Properties.Add(property);

            return this;
        }

        public abstract ReportCell GetCell(TSourceEntity entity);

        protected ReportCell CreateCell(TValue value, TSourceEntity entity)
        {
            // ReportCell cell = ReportCell.FromValue(value);
            this.reportCell.Clear();
            this.reportCell.SetValue(value);

            //// ReportCell cell = this.pool.GetOrCreate(
            ////     () => new ReportCell { Value = value },
            ////     c => c.Value = value);

            this.AddProperties();
            this.RunProcessors(entity);

            //// this.pool.Release(cell);

            return this.reportCell;
        }

        private void AddProperties()
        {
            this.reportCell.AddProperties(this.Properties);
        }

        private void RunProcessors(TSourceEntity entity)
        {
            for (int i = 0; i < this.Processors.Count; i++)
            {
                this.Processors[i].Process(this.reportCell, entity);
            }
        }
    }
}
