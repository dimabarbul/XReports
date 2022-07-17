using System;
using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;
using XReports.Utils;

namespace XReports.ReportCellsProviders
{
    public abstract class ReportCellsProvider<TSourceEntity, TValue> : IReportCellsProvider<TSourceEntity>
    {
        private readonly ReportCellsPool<ReportCell> pool = new ReportCellsPool<ReportCell>();

        protected ReportCellsProvider(string title)
        {
            this.Title = title;
        }

        public string Title { get; }

        public abstract Func<TSourceEntity, ReportCell> CellSelector { get; }

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

        protected ReportCell CreateCell(TValue value, TSourceEntity entity)
        {
            ReportCell cell = ReportCell.FromValue(value);
            //// ReportCell cell = this.pool.GetOrCreate(
            ////     () => new ReportCell { Value = value },
            ////     c => c.Value = value);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

            //// this.pool.Release(cell);

            return cell;
        }

        private void AddProperties(ReportCell cell)
        {
            cell.AddProperties(this.Properties);
        }

        private void RunProcessors(ReportCell cell, TSourceEntity entity)
        {
            for (int i = 0; i < this.Processors.Count; i++)
            {
                this.Processors[i].Process(cell, entity);
            }
        }
    }
}
