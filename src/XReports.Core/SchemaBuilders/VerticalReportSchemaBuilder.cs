using System;
using System.Linq;
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
            return this.InsertCellsProvider(index, title, provider);
        }

        public IReportSchemaCellsProviderBuilder<TSourceEntity> InsertColumnBefore(string beforeTitle, string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(beforeTitle), title, provider);
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

            return this.CellsProviders[index];
        }

        public VerticalReportSchema<TSourceEntity> BuildSchema()
        {
            return new VerticalReportSchema<TSourceEntity>(
                this.CellsProviders
                    .Select(c => c.Build(this.GlobalProperties))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.BuildComplexHeader(transpose: false),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
