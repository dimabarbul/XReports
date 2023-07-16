using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Helpers;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    /// <summary>
    /// Report schema builder.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public class ReportSchemaBuilder<TSourceItem> : IReportSchemaBuilder<TSourceItem>
    {
        private readonly List<IdentifiableCellProvider> cellProviders = new List<IdentifiableCellProvider>();
        private readonly IComplexHeaderBuilder complexHeaderBuilder = new ComplexHeaderBuilder();
        private readonly Dictionary<string, List<ReportCellProperty>> complexHeadersProperties = new Dictionary<string, List<ReportCellProperty>>();
        private readonly List<ReportCellProperty> commonComplexHeadersProperties = new List<ReportCellProperty>();
        private readonly List<ReportCellProperty> globalProperties = new List<ReportCellProperty>();
        private readonly List<IReportCellProcessor<TSourceItem>> globalProcessors = new List<IReportCellProcessor<TSourceItem>>();
        private readonly List<ReportTableProperty> tableProperties = new List<ReportTableProperty>();

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddGlobalProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.globalProperties.AddRange(properties);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddGlobalProcessors(params IReportCellProcessor<TSourceItem>[] processors)
        {
            this.ValidateAllItemsNotNull(processors);

            this.globalProcessors.AddRange(processors);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddTableProperties(params ReportTableProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.tableProperties.AddRange(properties);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeader(int rowIndex, string content, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, content, fromColumn, toColumn);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeader(int rowIndex, string content, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, content, fromColumn, toColumn);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeader(int rowIndex, string content, ColumnId fromColumn, ColumnId toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(
                rowIndex,
                content,
                new ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ColumnId(toColumn.Value));

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeader(int rowIndex, int rowSpan, string content, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, content, fromColumn, toColumn);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeader(int rowIndex, int rowSpan, string content, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, content, fromColumn, toColumn);

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeader(int rowIndex, int rowSpan, string content, ColumnId fromColumn, ColumnId toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(
                rowIndex,
                rowSpan,
                content,
                new ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ColumnId(toColumn.Value));

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeaderProperties(string content, params ReportCellProperty[] properties)
        {
            if (!this.complexHeadersProperties.ContainsKey(content))
            {
                this.complexHeadersProperties.Add(content, new List<ReportCellProperty>(properties));
            }
            else
            {
                this.complexHeadersProperties[content].AddRange(properties);
            }

            return this;
        }

        /// <inheritdoc />
        public IReportSchemaBuilder<TSourceItem> AddComplexHeaderProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllItemsNotNull(properties);

            this.commonComplexHeadersProperties.AddRange(properties);

            return this;
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> AddColumn(string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertColumn(this.cellProviders.Count, title, provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> AddColumn(ColumnId id, string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertColumn(this.cellProviders.Count, id, title, provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> InsertColumn(int index, string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertCellProvider(index, new CellProviderId(title), provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> InsertColumn(int index, ColumnId id, string title, IReportCellProvider<TSourceItem> provider)
        {
            Validation.NotNull(nameof(id), id);

            return this.InsertCellProvider(index, new CellProviderId(title, columnId: id), provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> InsertColumnBefore(string beforeTitle, string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertColumn(this.GetCellProviderIndex(beforeTitle), title, provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> InsertColumnBefore(ColumnId beforeId, string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertColumn(this.GetCellProviderIndex(beforeId), title, provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> InsertColumnBefore(string beforeTitle, ColumnId id, string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertColumn(this.GetCellProviderIndex(beforeTitle), id, title, provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> InsertColumnBefore(ColumnId beforeId, ColumnId id, string title, IReportCellProvider<TSourceItem> provider)
        {
            return this.InsertColumn(this.GetCellProviderIndex(beforeId), id, title, provider);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> ForColumn(string title)
        {
            return this.GetProvider(title);
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> ForColumn(int index)
        {
            if (index < 0 || index >= this.cellProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.cellProviders[index].Provider;
        }

        /// <inheritdoc />
        public IReportColumnBuilder<TSourceItem> ForColumn(ColumnId id)
        {
            Validation.NotNull(nameof(id), id);

            return this.cellProviders[this.GetCellProviderIndex(id)].Provider;
        }

        /// <inheritdoc />
        public IReportSchema<TSourceItem> BuildVerticalSchema()
        {
            if (this.cellProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no columns.");
            }

            return new VerticalReportSchema<TSourceItem>(
                this.cellProviders
                    .Select(c => c.Provider.Build(this.globalProperties, this.globalProcessors))
                    .ToArray(),
                this.tableProperties.ToArray(),
                this.BuildComplexHeader(this.cellProviders, transpose: false),
                this.complexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.commonComplexHeadersProperties.ToArray());
        }

        /// <inheritdoc />
        public IReportSchema<TSourceItem> BuildHorizontalSchema(int headerRowsCount)
        {
            if (this.cellProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no columns.");
            }

            Validation.NotNegative(nameof(headerRowsCount), headerRowsCount);

            if (headerRowsCount >= this.cellProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(headerRowsCount), $"Count of header rows {headerRowsCount} cannot be greater than or equal to rows count {this.cellProviders.Count}");
            }

            return new HorizontalReportSchema<TSourceItem>(
                this.cellProviders
                    .Take(headerRowsCount)
                    .Select(c => c.Provider.Build(Array.Empty<ReportCellProperty>(), Array.Empty<IReportCellProcessor<TSourceItem>>()))
                    .ToArray(),
                this.cellProviders
                    .Skip(headerRowsCount)
                    .Select(c => c.Provider.Build(this.globalProperties, this.globalProcessors))
                    .ToArray(),
                this.tableProperties.ToArray(),
                this.BuildComplexHeader(this.cellProviders.Skip(headerRowsCount).ToArray(), transpose: true),
                this.complexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.commonComplexHeadersProperties.ToArray());
        }

        private IReportColumnBuilder<TSourceItem> InsertCellProvider(int index, CellProviderId id, IReportCellProvider<TSourceItem> provider)
        {
            if (index < 0 || index > this.cellProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.ValidateIdUnique(id);

            ReportColumnBuilder<TSourceItem> builder = new ReportColumnBuilder<TSourceItem>(id.Title, provider);

            this.cellProviders.Insert(index, new IdentifiableCellProvider(id, builder));

            return builder;
        }

        private ComplexHeaderCell[,] BuildComplexHeader(
            IList<IdentifiableCellProvider> cellProviders,
            bool transpose)
        {
            ComplexHeaderCell[,] complexHeader = this.complexHeaderBuilder.Build(
                cellProviders.Select(p => p.Id.Title).ToArray(),
                this.GetColumnIds(cellProviders));

            return transpose ? complexHeader.Transpose() : complexHeader;
        }

        private void ValidateAllItemsNotNull<TItem>(IEnumerable<TItem> items)
        {
            if (items.Any(p => p == null))
            {
                throw new ArgumentException("All items should not be null", nameof(items));
            }
        }

        private void ValidateIdUnique(CellProviderId id)
        {
            if (id.ColumnId != null
                && this.cellProviders.Any(p => id.ColumnId.Equals(p.Id.ColumnId)))
            {
                throw new ArgumentException($"Column ID {id.ColumnId} already exists");
            }
        }

        private IReadOnlyList<ColumnId> GetColumnIds(IList<IdentifiableCellProvider> cellProviders)
        {
            return cellProviders
                .Select(p =>
                    p.Id.ColumnId != null ?
                        new ColumnId(p.Id.ColumnId.Value) :
                        null)
                .ToArray();
        }

        private IReportColumnBuilder<TSourceItem> GetProvider(string title)
        {
            return this.cellProviders[this.GetCellProviderIndex(title)].Provider;
        }

        private int GetCellProviderIndex(string title)
        {
            Validation.NotNull(nameof(title), title);

            int? index = null;
            for (int i = 0; i < this.cellProviders.Count; i++)
            {
                if (this.cellProviders[i].Id.Title.Equals(title, StringComparison.Ordinal))
                {
                    index = i;
                    break;
                }
            }
            if (index == null)
            {
                throw new ArgumentException($"Cells provider with title {title} is not found", nameof(title));
            }

            return (int)index;
        }

        private int GetCellProviderIndex(ColumnId columnId)
        {
            Validation.NotNull(nameof(columnId), columnId);

            int? index = null;
            for (int i = 0; i < this.cellProviders.Count; i++)
            {
                if (columnId.Equals(this.cellProviders[i].Id.ColumnId))
                {
                    index = i;
                    break;
                }
            }

            if (index == null)
            {
                throw new ArgumentException($"Cells provider with column ID {columnId} is not found", nameof(columnId));
            }

            return (int)index;
        }

        private class IdentifiableCellProvider
        {
            public IdentifiableCellProvider(CellProviderId id, IReportColumnBuilder<TSourceItem> provider)
            {
                this.Id = id;
                this.Provider = provider;
            }

            public CellProviderId Id { get; }
            public IReportColumnBuilder<TSourceItem> Provider { get; }
        }

        private class CellProviderId
        {
            public CellProviderId(string title, ColumnId columnId = null)
            {
                Validation.NotNull(nameof(title), title);

                this.Title = title;
                this.ColumnId = columnId;
            }

            public ColumnId ColumnId { get; }
            public string Title { get; }
        }
    }
}
