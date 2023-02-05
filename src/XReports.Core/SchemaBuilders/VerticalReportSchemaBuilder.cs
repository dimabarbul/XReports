using System;
using System.Linq;
using XReports.Helpers;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.SchemaBuilders
{
    public class VerticalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>, IVerticalReportSchemaBuilder<TSourceEntity>
    {
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

        public IVerticalReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, ColumnId fromColumn, ColumnId toColumn = null)
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

        public IVerticalReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, int rowSpan, string title, ColumnId fromColumn, ColumnId toColumn = null)
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

        public VerticalReportSchema<TSourceEntity> BuildSchema()
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
                this.BuildComplexHeader(transpose: false),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
