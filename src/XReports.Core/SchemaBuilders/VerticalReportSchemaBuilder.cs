using System.Linq;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;

namespace XReports.SchemaBuilders
{
    public class VerticalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>, IVerticalReportSchemaBuilder<TSourceEntity>
    {
        public IVerticalReportSchemaBuilder<TSourceEntity> AddColumn(IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.CellsProviders.Count, provider);
        }

        public IVerticalReportSchemaBuilder<TSourceEntity> InsertColumn(int index, IReportCellsProvider<TSourceEntity> provider)
        {
            this.InsertCellsProvider(index, provider);

            return this;
        }

        public IVerticalReportSchemaBuilder<TSourceEntity> InsertColumnBefore(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(title), provider);
        }

        public IVerticalReportSchemaBuilder<TSourceEntity> ForColumn(string title)
        {
            this.SelectProvider(title);

            return this;
        }

        public VerticalReportSchema<TSourceEntity> BuildSchema()
        {
            return new VerticalReportSchema<TSourceEntity>(
                this.CellsProviders
                    .Select(
                        c => new ReportSchemaCellsProvider<TSourceEntity>(
                            c.Provider,
                            this.AddGlobalProperties(c.CellProperties),
                            c.HeaderProperties.ToArray(),
                            c.CellProcessors.ToArray(),
                            c.HeaderProcessors.ToArray()))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.BuildComplexHeader(transpose: false),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
