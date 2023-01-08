using System;
using System.Collections.Generic;
using System.Linq;
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
            return this.InsertCellsProvider(index, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertRowBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertRow(this.GetCellsProviderIndex(beforeTitle), title, provider);
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

            return this.CellsProviders[index];
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
                    .Select(c => c.Build(this.GlobalProperties))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.BuildComplexHeader(transpose: true),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
