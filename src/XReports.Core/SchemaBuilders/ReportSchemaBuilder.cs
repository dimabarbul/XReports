using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Helpers;
using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders
{
    public class ReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        private readonly List<IdentifiableCellsProvider> cellsProviders = new List<IdentifiableCellsProvider>();
        private readonly IComplexHeaderBuilder complexHeaderBuilder = new ComplexHeaderBuilder();
        private readonly Dictionary<string, List<ReportCellProperty>> complexHeadersProperties = new Dictionary<string, List<ReportCellProperty>>();
        private readonly List<ReportCellProperty> commonComplexHeadersProperties = new List<ReportCellProperty>();
        private readonly List<ReportCellProperty> globalProperties = new List<ReportCellProperty>();
        private readonly List<ReportTableProperty> tableProperties = new List<ReportTableProperty>();

        public IReportSchemaBuilder<TSourceEntity> AddGlobalProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.globalProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportTableProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.tableProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.complexHeadersProperties.ContainsKey(title))
            {
                this.complexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.complexHeadersProperties[title].AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.commonComplexHeadersProperties.AddRange(properties);

            return this;
        }

        public IReportColumnBuilder<TSourceEntity> AddColumn(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.cellsProviders.Count, title, provider);
        }

        public IReportColumnBuilder<TSourceEntity> InsertColumn(int index, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertCellsProvider(index, new CellsProviderId(title), provider);
        }

        public IReportColumnBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeTitle), title, provider);
        }

        public IReportColumnBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeId), title, provider);
        }

        public IReportColumnBuilder<TSourceEntity> AddColumn(ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.cellsProviders.Count, id, title, provider);
        }

        public IReportColumnBuilder<TSourceEntity> InsertColumn(int index, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            Validation.NotNull(nameof(id), id);

            return this.InsertCellsProvider(index, new CellsProviderId(title, columnId: id), provider);
        }

        public IReportColumnBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeTitle), id, title, provider);
        }

        public IReportColumnBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeId), id, title, provider);
        }

        public IReportColumnBuilder<TSourceEntity> ForColumn(string title)
        {
            return this.GetProvider(title);
        }

        public IReportColumnBuilder<TSourceEntity> ForColumn(int index)
        {
            if (index < 0 || index >= this.cellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.cellsProviders[index].Provider;
        }

        public IReportColumnBuilder<TSourceEntity> ForColumn(ColumnId id)
        {
            Validation.NotNull(nameof(id), id);

            return this.cellsProviders[this.GetCellsProviderIndex(id)].Provider;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, ColumnId fromColumn, ColumnId toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(
                rowIndex,
                title,
                new ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ColumnId(toColumn.Value));

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, ColumnId fromColumn, ColumnId toColumn = null)
        {
            this.complexHeaderBuilder.AddGroup(
                rowIndex,
                rowSpan,
                title,
                new ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ColumnId(toColumn.Value));

            return this;
        }

        public IReportSchema<TSourceEntity> BuildVerticalSchema()
        {
            if (this.cellsProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no columns.");
            }

            return new VerticalReportSchema<TSourceEntity>(
                this.cellsProviders
                    .Select(c => c.Provider.Build(this.globalProperties))
                    .ToArray(),
                this.tableProperties.ToArray(),
                this.BuildComplexHeader(this.cellsProviders, transpose: false),
                this.complexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.commonComplexHeadersProperties.ToArray());
        }

        public IReportSchema<TSourceEntity> BuildHorizontalSchema(int headerRowsCount)
        {
            if (this.cellsProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no columns.");
            }

            Validation.NotNegative(nameof(headerRowsCount), headerRowsCount);

            if (headerRowsCount >= this.cellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(headerRowsCount), $"Count of header rows {headerRowsCount} cannot be greater than or equal to rows count {this.cellsProviders.Count}");
            }

            return new HorizontalReportSchema<TSourceEntity>(
                this.cellsProviders
                    .Take(headerRowsCount)
                    .Select(c => c.Provider.Build(Array.Empty<ReportCellProperty>()))
                    .ToArray(),
                this.cellsProviders
                    .Skip(headerRowsCount)
                    .Select(c => c.Provider.Build(this.globalProperties))
                    .ToArray(),
                this.tableProperties.ToArray(),
                this.BuildComplexHeader(this.cellsProviders.Skip(headerRowsCount).ToArray(), transpose: true),
                this.complexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.commonComplexHeadersProperties.ToArray());
        }

        protected IReportColumnBuilder<TSourceEntity> InsertCellsProvider(int index, CellsProviderId id, IReportCellsProvider<TSourceEntity> provider)
        {
            if (index < 0 || index > this.cellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.ValidateIdUnique(id);

            ReportColumnBuilder<TSourceEntity> builder = new ReportColumnBuilder<TSourceEntity>(id.Title, provider);

            this.cellsProviders.Insert(index, new IdentifiableCellsProvider(id, builder));

            return builder;
        }

        protected ComplexHeaderCell[,] BuildComplexHeader(
            IList<IdentifiableCellsProvider> cellsProviders,
            bool transpose)
        {
            ComplexHeaderCell[,] complexHeader = this.complexHeaderBuilder.Build(
                cellsProviders.Select(p => p.Id.Title).ToArray(),
                this.GetColumnIds(cellsProviders));

            return transpose ? complexHeader.Transpose() : complexHeader;
        }

        private void ValidateAllPropertiesNotNull<TProperty>(TProperty[] properties)
        {
            if (properties.Any(p => p == null))
            {
                throw new ArgumentException("All properties should not be null", nameof(properties));
            }
        }

        private void ValidateIdUnique(CellsProviderId id)
        {
            if (id.ColumnId != null
                && this.cellsProviders.Any(p => id.ColumnId.Equals(p.Id.ColumnId)))
            {
                throw new ArgumentException($"Column ID {id.ColumnId} already exists");
            }
        }

        private IReadOnlyList<ColumnId> GetColumnIds(
            IList<IdentifiableCellsProvider> cellsProviders)
        {
            return cellsProviders
                .Select(p =>
                    p.Id.ColumnId != null ?
                        new ColumnId(p.Id.ColumnId.Value) :
                        null)
                .ToArray();
        }

        private IReportColumnBuilder<TSourceEntity> GetProvider(string title)
        {
            return this.cellsProviders[this.GetCellsProviderIndex(title)].Provider;
        }

        private int GetCellsProviderIndex(string title)
        {
            Validation.NotNull(nameof(title), title);

            int? index = null;
            for (int i = 0; i < this.cellsProviders.Count; i++)
            {
                if (this.cellsProviders[i].Id.Title.Equals(title, StringComparison.Ordinal))
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

        private int GetCellsProviderIndex(ColumnId columnId)
        {
            Validation.NotNull(nameof(columnId), columnId);

            int? index = null;
            for (int i = 0; i < this.cellsProviders.Count; i++)
            {
                if (columnId.Equals(this.cellsProviders[i].Id.ColumnId))
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

        protected class IdentifiableCellsProvider
        {
            public IdentifiableCellsProvider(CellsProviderId id, IReportColumnBuilder<TSourceEntity> provider)
            {
                this.Id = id;
                this.Provider = provider;
            }

            public CellsProviderId Id { get; }
            public IReportColumnBuilder<TSourceEntity> Provider { get; }
        }

        protected class CellsProviderId
        {
            public CellsProviderId(string title, ColumnId columnId = null)
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
