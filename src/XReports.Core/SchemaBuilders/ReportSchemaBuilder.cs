using System;
using System.Collections.Generic;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellProcessors;

namespace XReports.SchemaBuilders
{
    public abstract class ReportSchemaBuilder<TSourceEntity>
    {
        private ConfiguredCellsProvider currentProvider;

        protected List<ConfiguredCellsProvider> CellsProviders { get; } = new List<ConfiguredCellsProvider>();

        protected Dictionary<string, ConfiguredCellsProvider> NamedProviders { get; } = new Dictionary<string, ConfiguredCellsProvider>();

        protected List<ComplexHeader> ComplexHeaders { get; } = new List<ComplexHeader>();

        protected Dictionary<string, List<ReportCellProperty>> ComplexHeadersProperties { get; } = new Dictionary<string, List<ReportCellProperty>>();

        protected List<ReportCellProperty> CommonComplexHeadersProperties { get; } = new List<ReportCellProperty>();

        protected List<ReportCellProperty> TableProperties { get; } = new List<ReportCellProperty>();

        public ReportSchemaBuilder<TSourceEntity> AddAlias(string alias)
        {
            this.NamedProviders[alias] = this.currentProvider;

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddAlias(string alias, string target)
        {
            this.NamedProviders[alias] = this.NamedProviders[target];

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddTableProperties(params ReportCellProperty[] properties)
        {
            this.TableProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddProperties(params ReportCellProperty[] properties)
        {
            this.currentProvider.CellProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddDynamicProperty(Func<TSourceEntity, ReportCellProperty> propertySelector)
        {
            this.currentProvider.CellProcessors.Add(new DynamicPropertiesCellProcessor<TSourceEntity>(propertySelector));

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddHeaderProperties(params ReportCellProperty[] properties)
        {
            this.currentProvider.HeaderProperties.AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentProvider.CellProcessors.AddRange(processors);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddHeaderProcessors(params IReportCellProcessor<TSourceEntity>[] processors)
        {
            this.currentProvider.HeaderProcessors.AddRange(processors);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, int fromColumn, int? toColumn = null)
        {
            this.ComplexHeaders.Add(
                new ComplexHeader()
                {
                    RowIndex = rowIndex,
                    Title = title,
                    StartIndex = fromColumn,
                    EndIndex = toColumn ?? fromColumn,
                });

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddComplexHeader(int rowIndex, string title, string fromColumn, string toColumn = null)
        {
            this.ComplexHeaders.Add(
                new ComplexHeader()
                {
                    RowIndex = rowIndex,
                    Title = title,
                    StartIndex = this.CellsProviders.FindIndex(c => c.Provider.Title.Equals(fromColumn, StringComparison.OrdinalIgnoreCase)),
                    EndIndex = this.CellsProviders.FindIndex(c => c.Provider.Title.Equals(toColumn ?? fromColumn, StringComparison.OrdinalIgnoreCase)),
                });

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(string title, params ReportCellProperty[] properties)
        {
            if (!this.ComplexHeadersProperties.ContainsKey(title))
            {
                this.ComplexHeadersProperties.Add(title, new List<ReportCellProperty>());
            }

            this.ComplexHeadersProperties[title].AddRange(properties);

            return this;
        }

        public ReportSchemaBuilder<TSourceEntity> AddComplexHeaderProperties(params ReportCellProperty[] properties)
        {
            this.CommonComplexHeadersProperties.AddRange(properties);

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> SelectProvider(ConfiguredCellsProvider provider)
        {
            this.currentProvider = provider;

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> SelectProvider(int index)
        {
            this.currentProvider = this.CellsProviders[index];

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> SelectProvider(string title)
        {
            this.currentProvider = this.CellsProviders[this.GetCellsProviderIndex(title)];

            return this;
        }

        protected ReportSchemaBuilder<TSourceEntity> InsertCellsProvider(int index, IReportCellsProvider<TSourceEntity> provider)
        {
            this.currentProvider = new ConfiguredCellsProvider(provider);

            this.CellsProviders.Insert(index, this.currentProvider);
            this.NamedProviders[this.currentProvider.Provider.Title] = this.currentProvider;

            return this;
        }

        protected int GetCellsProviderIndex(string title)
        {
            return this.CellsProviders.IndexOf(this.NamedProviders[title]);
        }

        protected class ConfiguredCellsProvider
        {
            public ConfiguredCellsProvider(IReportCellsProvider<TSourceEntity> provider)
            {
                this.Provider = provider;
            }

            public IReportCellsProvider<TSourceEntity> Provider { get; set; }

            public List<ReportCellProperty> CellProperties { get; set; } = new List<ReportCellProperty>();

            public List<ReportCellProperty> HeaderProperties { get; set; } = new List<ReportCellProperty>();

            public List<IReportCellProcessor<TSourceEntity>> CellProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();

            public List<IReportCellProcessor<TSourceEntity>> HeaderProcessors { get; set; } = new List<IReportCellProcessor<TSourceEntity>>();
        }
    }
}
