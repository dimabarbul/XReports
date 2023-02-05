using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Helpers;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportSchemaCellsProviders;

namespace XReports.SchemaBuilders
{
    public class HorizontalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>, IHorizontalReportSchemaBuilder<TSourceEntity>
    {
        private readonly List<IReportSchemaCellsProviderBuilder<TSourceEntity>> headerProviders = new List<IReportSchemaCellsProviderBuilder<TSourceEntity>>();

        public IReportSchemaCellsProviderBuilder<TSourceEntity> AddRow(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.CellsProviders.Count, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRow(int index, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertCellsProvider(index, new CellsProviderId(title), provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.GetCellsProviderIndex(beforeTitle), title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(RowId beforeId, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.GetCellsProviderIndex(beforeId), title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> AddRow(RowId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.CellsProviders.Count, id, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRow(int index, RowId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            Validation.NotNull(nameof(id), id);

            return this.InsertCellsProvider(index, new CellsProviderId(title, rowId: id), provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(string beforeTitle, RowId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.GetCellsProviderIndex(beforeTitle), id, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(RowId beforeId, RowId id, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.GetCellsProviderIndex(beforeId), id, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(string title)
        {
            return this.GetProvider(title);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(int index)
        {
            if (index < 0 || index >= this.CellsProviders.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.CellsProviders[index].Provider;
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForRow(RowId id)
        {
            Validation.NotNull(nameof(id), id);

            return this.CellsProviders[this.GetCellsProviderIndex(id)].Provider;
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> AddHeaderRow(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertHeaderRow(this.headerProviders.Count, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertHeaderRow(int rowIndex, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            if (title == null)
            {
                throw new ArgumentException("Title cannot be null", nameof(title));
            }

            IReportSchemaCellsProviderBuilder<TSourceEntity> builder = new ReportSchemaCellsProviderBuilder<TSourceEntity>(title, provider);
            this.headerProviders.Insert(rowIndex, builder);

            return builder;
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> ForHeaderRow(int index)
        {
            return this.headerProviders[index];
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, RowId fromColumn, RowId toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(
                rowIndex,
                title,
                new ComplexHeader.ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ComplexHeader.ColumnId(toColumn.Value));

            return this;
        }

        public IHorizontalReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, RowId fromColumn, RowId toColumn = null)
        {
            this.ComplexHeaderBuilder.AddGroup(
                rowIndex,
                rowSpan,
                title,
                new ComplexHeader.ColumnId(fromColumn.Value),
                toColumn == null ?
                    null :
                    new ComplexHeader.ColumnId(toColumn.Value));

            return this;
        }

        public HorizontalReportSchema<TSourceEntity> BuildSchema()
        {
            if (this.CellsProviders.Count == 0)
            {
                throw new InvalidOperationException("Cannot build schema for table with no rows.");
            }

            return new HorizontalReportSchema<TSourceEntity>(
                this.headerProviders
                    .Select(c => c.Build(Array.Empty<ReportCellProperty>()))
                    .ToArray(),
                this.CellsProviders
                    .Select(c => c.Provider.Build(this.GlobalProperties))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.BuildComplexHeader(transpose: true),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
