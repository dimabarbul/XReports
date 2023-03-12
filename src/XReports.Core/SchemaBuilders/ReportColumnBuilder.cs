using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    public class ReportColumnBuilder<TSourceEntity> : IReportColumnBuilder<TSourceEntity>
    {
        private readonly string title;
        private readonly IReportCellsProvider<TSourceEntity> provider;
        private readonly List<ReportCellProperty> cellProperties = new List<ReportCellProperty>();
        private readonly List<ReportCellProperty> headerProperties = new List<ReportCellProperty>();
        private readonly List<IReportCellProcessor<TSourceEntity>> cellProcessors = new List<IReportCellProcessor<TSourceEntity>>();
        private readonly List<IReportCellProcessor<TSourceEntity>> headerProcessors = new List<IReportCellProcessor<TSourceEntity>>();

        public ReportColumnBuilder(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            this.title = title;
            this.provider = provider;
        }

        public IReportColumnBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.cellProperties.AddRange(properties);

            return this;
        }

        public IReportColumnBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.headerProperties.AddRange(properties);

            return this;
        }

        public IReportColumnBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.ValidateAllItemsNotNull(processors);

            this.cellProcessors.AddRange(processors);

            return this;
        }

        public IReportColumnBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.ValidateAllItemsNotNull(processors);

            this.headerProcessors.AddRange(processors);

            return this;
        }

        public IReportColumn<TSourceEntity> Build(IReadOnlyList<ReportCellProperty> globalProperties)
        {
            this.ValidateAllItemsNotNull(globalProperties);

            return new ReportColumn<TSourceEntity>(
                this.title,
                this.provider,
                this.MergeGlobalProperties(globalProperties),
                this.headerProperties.ToArray(),
                this.cellProcessors.ToArray(),
                this.headerProcessors.ToArray());
        }

        private ReportCellProperty[] MergeGlobalProperties(IReadOnlyList<ReportCellProperty> globalProperties)
        {
            List<ReportCellProperty> result = new List<ReportCellProperty>(this.cellProperties);
            for (int i = 0; i < globalProperties.Count; i++)
            {
                if (!result.Any(p => p.GetType() == globalProperties[i].GetType()))
                {
                    result.Add(globalProperties[i]);
                }
            }

            return result.ToArray();
        }

        private void ValidateAllItemsNotNull<TItem>(IEnumerable<TItem> items)
        {
            if (items.Any(i => i == null))
            {
                throw new ArgumentException("All items should not be null", nameof(items));
            }
        }
    }
}
