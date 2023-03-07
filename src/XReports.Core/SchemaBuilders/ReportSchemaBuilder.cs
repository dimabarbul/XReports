using System;
using System.Collections.Generic;
using System.Linq;
using XReports.ComplexHeader;
using XReports.Extensions;
using XReports.Helpers;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportSchemaCellsProviders;

namespace XReports.SchemaBuilders
{
    public class ReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
    {
        protected ComplexHeaderBuilder ComplexHeaderBuilder { get; } = new ComplexHeaderBuilder();

        protected List<IdentifiableCellsProvider> CellsProviders { get; } = new List<IdentifiableCellsProvider>();

        protected Dictionary<string, List<ReportCellProperty>> ComplexHeadersProperties { get; } = new Dictionary<string, List<ReportCellProperty>>();

        protected List<ReportCellProperty> CommonComplexHeadersProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportCellProperty> GlobalProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportTableProperty> TableProperties { get; } = new List<ReportTableProperty>();

        public IReportSchemaBuilder<TSourceEntity> AddGlobalProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.GlobalProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportTableProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.TableProperties.AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(rowIndex, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, int fromColumn, int? toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, string fromColumn, string toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(rowIndex, rowSpan, title, fromColumn, toColumn);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.ComplexHeadersProperties.ContainsKey(title))
            {
                this.ComplexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.ComplexHeadersProperties[title].AddRange(properties);

            return this;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties)
        {
            this.ValidateAllPropertiesNotNull(properties);

            this.CommonComplexHeadersProperties.AddRange(properties);

            return this;
        }

        protected IReportSchemaCellsProviderBuilder<TSourceEntity> GetProvider(string title)
        {
            return this.CellsProviders[this.GetCellsProviderIndex(title)].Provider;
        }

        protected ReportSchemaCellsProviderBuilder<TSourceEntity> InsertCellsProvider(int index, CellsProviderId id, IReportCellsProvider<TSourceEntity> provider)
        {
            if (index < 0 || index > this.CellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.ValidateIdUnique(id);

            ReportSchemaCellsProviderBuilder<TSourceEntity> builder = new ReportSchemaCellsProviderBuilder<TSourceEntity>(id.Title, provider);

            this.CellsProviders.Insert(index, new IdentifiableCellsProvider(id, builder));

            return builder;
        }

        protected int GetCellsProviderIndex(string title)
        {
            Validation.NotNull(nameof(title), title);

            int? index = null;
            for (int i = 0; i < this.CellsProviders.Count; i++)
            {
                if (this.CellsProviders[i].Id.Title.Equals(title, StringComparison.Ordinal))
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

        protected int GetCellsProviderIndex(ColumnId columnId)
        {
            Validation.NotNull(nameof(columnId), columnId);

            int? index = null;
            for (int i = 0; i < this.CellsProviders.Count; i++)
            {
                if (columnId.Equals(this.CellsProviders[i].Id.ColumnId))
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

        protected ComplexHeaderCell[,] BuildComplexHeader(
            IList<IdentifiableCellsProvider> cellsProviders,
            bool transpose)
        {
            ComplexHeaderCell[,] complexHeader = this.ComplexHeaderBuilder.Build(
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
                && this.CellsProviders.Any(p => id.ColumnId.Equals(p.Id.ColumnId)))
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

        protected class IdentifiableCellsProvider
        {
            public IdentifiableCellsProvider(CellsProviderId id, IReportSchemaCellsProviderBuilder<TSourceEntity> provider)
            {
                this.Id = id;
                this.Provider = provider;
            }

            public CellsProviderId Id { get; }
            public IReportSchemaCellsProviderBuilder<TSourceEntity> Provider { get; }
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

        public IReportSchemaCellsProviderBuilder<TSourceEntity> AddColumn(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.CellsProviders.Count, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumn(int index, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertCellsProvider(index, new CellsProviderId(title), provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeTitle), title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeId), title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> AddColumn(ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.CellsProviders.Count, id, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumn(int index, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            Validation.NotNull(nameof(id), id);

            return this.InsertCellsProvider(index, new CellsProviderId(title, columnId: id), provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeTitle), id, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(ColumnId beforeId, ColumnId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeId), id, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(string title)
        {
            return this.GetProvider(title);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(int index)
        {
            if (index < 0 || index >= this.CellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.CellsProviders[index].Provider;
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForColumn(ColumnId id)
        {
            Validation.NotNull(nameof(id), id);

            return this.CellsProviders[this.GetCellsProviderIndex(id)].Provider;
        }

        public IReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, ColumnId fromColumn, ColumnId toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(
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
            this.ComplexHeaderBuilder.AddGroup(
                rowIndex,
                rowSpan,
                title,
                new ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ColumnId(toColumn.Value));

            return this;
        }

        public VerticalReportSchema<TSourceEntity> BuildVerticalSchema()
        {
            if (this.CellsProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no columns.");
            }

            return new VerticalReportSchema<TSourceEntity>(
                this.CellsProviders
                    .Select(c => c.Provider.Build(this.GlobalProperties))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.BuildComplexHeader(this.CellsProviders, transpose: false),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }

        public HorizontalReportSchema<TSourceEntity> BuildHorizontalSchema(int headerRowsCount)
        {
            if (this.CellsProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no columns.");
            }

            Validation.NotNegative(nameof(headerRowsCount), headerRowsCount);

            if (headerRowsCount >= this.CellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(headerRowsCount), $"Count of header rows {headerRowsCount} cannot be greater than or equal to rows count {this.CellsProviders.Count}");
            }

            return new HorizontalReportSchema<TSourceEntity>(
                this.CellsProviders
                    .Take(headerRowsCount)
                    .Select(c => c.Provider.Build(Array.Empty<ReportCellProperty>()))
                    .ToArray(),
                this.CellsProviders
                    .Skip(headerRowsCount)
                    .Select(c => c.Provider.Build(this.GlobalProperties))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.BuildComplexHeader(this.CellsProviders.Skip(headerRowsCount).ToArray(), transpose: true),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
