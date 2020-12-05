using System.Collections.Generic;
using Reports.Core.Interfaces;
using Reports.Core.Models;

namespace Reports.Core.SchemaBuilders
{
    public abstract class ReportSchemaBuilder<TSourceEntity>
    {
        protected readonly List<ConfiguredCellsProvider> CellsProviders = new List<ConfiguredCellsProvider>();
        protected readonly Dictionary<string, ConfiguredCellsProvider> NamedProviders = new Dictionary<string, ConfiguredCellsProvider>();
        protected readonly List<ConfiguredCellsProvider> HeaderProviders = new List<ConfiguredCellsProvider>();
        protected ConfiguredCellsProvider CurrentProvider;

        protected class ConfiguredCellsProvider
        {
            public IReportCellsProvider<TSourceEntity> Provider { get; set; }
            public List<ReportCellProperty> CellProperties { get; set; } = new List<ReportCellProperty>();
            public List<ReportCellProperty> HeaderProperties { get; set; } = new List<ReportCellProperty>();
            public List<IReportCellProcessor<TSourceEntity>> CellProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();
            public List<IReportCellProcessor<TSourceEntity>> HeaderProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();

            public ConfiguredCellsProvider(IReportCellsProvider<TSourceEntity> provider)
            {
                this.Provider = provider;
            }
        }

        public ReportSchemaBuilder<TSourceEntity> AddAlias(string alias)
        {
            this.NamedProviders[alias] = this.CurrentProvider;

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddAlias(string alias, string target)
        {
            this.NamedProviders[alias] = this.NamedProviders[target];

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.CurrentProvider.CellProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.CurrentProvider.HeaderProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.CurrentProvider.CellProcessors.AddRange(processors);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.CurrentProvider.HeaderProcessors.AddRange(processors);

            return this;
        }
    }
}