using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Report column builder.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public class ReportColumnBuilder<TSourceItem> : IReportColumnBuilder<TSourceItem>
    {
        private readonly string title;
        private readonly IReportCellProvider<TSourceItem> provider;
        private readonly List<ReportCellProperty> cellProperties = new List<ReportCellProperty>();
        private readonly List<ReportCellProperty> headerProperties = new List<ReportCellProperty>();
        private readonly List<IReportCellProcessor<TSourceItem>> cellProcessors = new List<IReportCellProcessor<TSourceItem>>();
        private readonly List<IHeaderReportCellProcessor> headerProcessors = new List<IHeaderReportCellProcessor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportColumnBuilder{TSourceItem}" /> class.
        /// </summary>
        /// <param name="title">Report column title. Will be used as content of header cell.</param>
        /// <param name="provider">Report cell provider to use to create cells.</param>
        public ReportColumnBuilder(string title, IReportCellProvider<TSourceItem> provider)
        {
            this.title = title;
            this.provider = provider;
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> AddProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.cellProperties.AddRange(properties);

            return this;
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.headerProperties.AddRange(properties);

            return this;
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> AddProcessors(params IReportCellProcessor<TSourceItem>[] processors)
        {
            this.ValidateAllItemsNotNull(processors);

            this.cellProcessors.AddRange(processors);

            return this;
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> AddHeaderProcessors(params IHeaderReportCellProcessor[] processors)
        {
            this.ValidateAllItemsNotNull(processors);

            this.headerProcessors.AddRange(processors);

            return this;
        }

        /// <inheritdoc />
        public IReportColumn<TSourceItem> Build(
            IReadOnlyList<ReportCellProperty> globalProperties,
            IReadOnlyList<IReportCellProcessor<TSourceItem>> globalProcessors)
        {
            this.ValidateAllItemsNotNull(globalProperties);
            this.ValidateAllItemsNotNull(globalProcessors);

            return new ReportColumn<TSourceItem>(
                this.title,
                this.provider,
                this.MergeGlobalProperties(globalProperties),
                this.headerProperties.ToArray(),
                this.MergeGlobalProcessors(globalProcessors),
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

        private IReportCellProcessor<TSourceItem>[] MergeGlobalProcessors(IReadOnlyList<IReportCellProcessor<TSourceItem>> globalProcessors)
        {
            List<IReportCellProcessor<TSourceItem>> result = new List<IReportCellProcessor<TSourceItem>>(this.cellProcessors);
            for (int i = 0; i < globalProcessors.Count; i++)
            {
                if (!result.Any(p => p.GetType() == globalProcessors[i].GetType()))
                {
                    result.Add(globalProcessors[i]);
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
