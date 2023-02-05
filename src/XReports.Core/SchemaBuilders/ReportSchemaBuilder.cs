using System;
using System.Collections.Generic;
using System.Linq;
using XReports.ComplexHeader;
using XReports.Extensions;
using XReports.Helpers;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportSchemaCellsProviders;
using ColumnId = XReports.Models.ColumnId;

namespace XReports.SchemaBuilders
{
    public abstract class ReportSchemaBuilder<TSourceEntity> : IReportSchemaBuilder<TSourceEntity>
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

        protected int GetCellsProviderIndex(RowId rowId)
        {
            Validation.NotNull(nameof(rowId), rowId);

            int? index = null;
            for (int i = 0; i < this.CellsProviders.Count; i++)
            {
                if (rowId.Equals(this.CellsProviders[i].Id.RowId))
                {
                    index = i;
                    break;
                }
            }

            if (index == null)
            {
                throw new ArgumentException($"Cells provider with row ID {rowId} is not found", nameof(rowId));
            }

            return (int)index;
        }

        protected ComplexHeaderCell[,] BuildComplexHeader(bool transpose)
        {
            ComplexHeaderCell[,] complexHeader = this.ComplexHeaderBuilder.Build(
                this.CellsProviders.Select(p => p.Id.Title).ToArray(),
                this.GetColumnIds());

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

            if (id.RowId != null
                && this.CellsProviders.Any(p => id.RowId.Equals(p.Id.RowId)))
            {
                throw new ArgumentException($"Row ID {id.RowId} already exists");
            }
        }

        private IReadOnlyList<ComplexHeader.ColumnId> GetColumnIds()
        {
            return this.CellsProviders
                .Select(p =>
                    p.Id.ColumnId != null ?
                        new ComplexHeader.ColumnId(p.Id.ColumnId.Value) :
                        (p.Id.RowId != null ?
                            new ComplexHeader.ColumnId(p.Id.RowId.Value) :
                            null))
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
            public CellsProviderId(string title, ColumnId columnId = null, RowId rowId = null)
            {
                if (columnId != null && rowId != null)
                {
                    throw new ArgumentException($"Only one of {nameof(columnId)} and {nameof(rowId)} can be specified");
                }

                Validation.NotNull(nameof(title), title);

                this.Title = title;
                this.ColumnId = columnId;
                this.RowId = rowId;
            }

            public ColumnId ColumnId { get; }
            public RowId RowId { get; }
            public string Title { get; }
        }
    }
}
